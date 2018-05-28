using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.SPlusTools.Data.Entities;

namespace UvA.SPlusTools.Data.Tasks
{
    public class MergeLocationsTask : ImageTask
    {
        public string Suffixes { get; set; }
        public string MergeInto { get; set; }
        public bool DeleteOthers { get; set; }
        public bool RemoveSuffix { get; set; }

        int Progress;

        dynamic College;

        public MergeLocationsTask()
        {

        }

        public override void Execute()
        {
            College = new College(ProgID).Object;

            string[] suffixes = Suffixes.Split(',');
            List<LocationWrapper> locs = new List<LocationWrapper>();
            double total = College.Locations.Count; double count = 0;
            foreach (var loc in College.Locations)
            {
                Progress = (int)Math.Round(100.0 * ++count / total);
                var suffix = suffixes.FirstOrDefault(s => loc.Name.EndsWith(s));
                if (suffix != null)
                    locs.Add(new LocationWrapper(loc, suffix));
            }
            count = 0;
            foreach (var loc in College.Locations)
            {
                Progress = (int)Math.Round(100.0 * ++count / total);
                string name = loc.Name;
                if (locs.Any(l => l.NamePart == name))
                    locs.Add(new LocationWrapper(loc, ""));
            }
            
            Func<string, dynamic> findLoc = part =>
            {
                string parentName = string.IsNullOrEmpty(MergeInto) ? part
                    : locs.First(l => l.NamePart == part && l.Suffix == MergeInto).Name;
                var pLocs = College.Locations.Find(null, parentName);
                if (pLocs.Count == 0)
                {
                    var zloc = locs.FirstOrDefault(l => l.NamePart == part && !l.HostKey.StartsWith("#"));
                    if (zloc == null)
                        zloc = locs.FirstOrDefault(l => l.NamePart == part);
                    pLocs = College.Locations.Find(null, zloc.Name);
                }
                return pLocs[1];

            };

            total = College.Activities.Count; count = 0;
            foreach (var act in College.Activities)
            {
                Progress = (int)Math.Round(100.0 * ++count / total);
                if (act.SchedulingStatus != 0)
                    continue;

                var res = act.GetResourceAllocation(1);
                var resList = ToList(res);

                bool change = false;
                foreach (var loc in resList)
                {
                    string locName = loc.Name;
                    var tl = locs.FirstOrDefault(l => l.Name == locName);
                    if (tl == null)
                        continue;
                    var parentLoc = findLoc(tl.NamePart);
                    if (parentLoc.ObjectId == loc.ObjectId)
                        continue;
                    change = true;
                    res.Remove(loc);
                    res.Add(parentLoc);
                }
                if (!change)
                    continue;

                var setting = College.CreateSchedulingResourceSetting();
                setting.UseLocation = true;
                var reqType = act.GetLocationRequirementType;
                act.UnscheduleSpecialMany(setting);
                act.SetResourceRequirement(1, 1, res.Count, res);
                act.ScheduleSpecialMany(setting);
                if ((int)reqType != 1)
                    act.SetResourceRequirement(1, 2, res.Count, res);
                //Log.Add(Description, "Reschedule", act);
            }

            total = College.Activities.Count; count = 0;
            foreach (var act in College.Activities)
            {
                Progress = (int)Math.Round(100.0 * ++count / total);
                if (act.GetResourceRequirementType(1) != 1)
                    continue;
                
                var res = act.GetResourceRequirement(1);
                var resList = ToList(res);

                bool change = false;
                foreach (var loc in resList)
                {
                    string locName = loc.Name;
                    var tl = locs.FirstOrDefault(l => l.Name == locName);
                    if (tl == null)
                        continue;
                    var parentLoc = findLoc(tl.NamePart);
                    if (loc.ObjectId == parentLoc.ObjectId)
                        continue;
                    change = true;
                    res.Remove(loc);
                    res.Add(parentLoc);
                }
                if (!change)
                    continue;

                //Log.Add(Description, "ChangePreset", act);
                act.SetResourceRequirement(1, 1, res.Count, res);
            }

            total = locs.Count; count = 0;
            foreach (var loc in locs)
            {
                Progress = (int)Math.Round(100.0 * ++count / total);
                var zloc = locs.FirstOrDefault(l => l.NamePart == loc.NamePart && !l.Name.EndsWith(")"));
                if (zloc == null)
                    zloc = locs.First(l => l.NamePart == loc.NamePart);
                if (DeleteOthers && ((MergeInto != null && loc.Suffix != MergeInto) || (MergeInto == null && loc != zloc)))
                    College.DeleteLocation(loc.Location);
                else if (RemoveSuffix)
                    loc.Location.Name = loc.NamePart;
            }
        }

        protected List<dynamic> ToList(dynamic obj)
        {
            List<dynamic> list = new List<dynamic>();
            foreach (var item in obj)
                list.Add(item);
            return list;
        }

        class LocationWrapper
        {
            public dynamic Location { get; set; }
            public string Name { get; set; }
            public string HostKey { get; set; }
            public string Suffix { get; set; }
            public string NamePart { get { return Name.Substring(0, Name.Length - Suffix.Length); } }

            public LocationWrapper(dynamic loc, string suffix)
            {
                Suffix = suffix;
                Location = loc;
                Name = Location.Name;
                HostKey = Location.HostKey;
            }
        }
    }
}
