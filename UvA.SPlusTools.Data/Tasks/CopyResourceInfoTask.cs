using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.SPlusTools.Data.Entities;
using UvA.Utilities;

namespace UvA.SPlusTools.Data.Tasks
{
    /// <summary>
    /// Copy resource data between two images
    /// </summary>
    public class CopyResourceInfoTask : TwoImageTask
    {
        static readonly string[] AllProperties = new string[] { "HostKey", "Name", "Description", "UserText1", "UserText2", "UserText3",
            "UserText4", "UserText5", "Availability", "Suitabilities", "Capacity", "AvoidConcurrency", "Zone", "Department", "Email", "Tags", "SharedWith" };

        public ResourceType TargetType { get; set; }
        public string[] HostKeys { get; set; }
        public string[] Properties { get; set; }
        public string GroupName { get; set; }

        College Source, Destination;

        public override void Execute()
        {
            Source = new College(SourceProgID);
            Destination = new College(DestinationProgID);

            IEnumerable<IResourceObject> group = null;
            if (!string.IsNullOrEmpty(GroupName))
            {
                GroupName = GroupName.Replace("{DATE}", DateTime.Now.ToString("yyyyMMdd")).Replace("{TIME}", DateTime.Now.ToString("HH_mm"));
                if (TargetType == ResourceType.Location)
                {
                    var gr = new LocationGroup(Destination) { Name = GroupName, Description = "CopyResourceInfoTask" };
                    group = gr.Members;
                }
                else
                {
                    var gr = new StaffMemberGroup(Destination) { Name = GroupName, Description = "CopyResourceInfoTask" };
                    group = gr.Members;
                }
            }

            var dict = new Dictionary<string, IResourceObject>();
            foreach (var key in HostKeys)
            {
                var sRes = GetObject(Source, key);
                if (sRes == null)
                    throw new InvalidOperationException("Object " + key + " does not exist");
                var dRes = GetObject(Destination, key);
                bool isNew = dRes == null;

                Log.WriteLine("{2} {0} ({1})", key, sRes.Name, isNew ? "Creating" : "Updating");
                if (isNew)
                    dRes = TargetType == ResourceType.Staff ? (IResourceObject)new StaffMember(Destination) : new Location(Destination);
                dict.Add(key, dRes);
                if (group != null)
                    AddObject(group, dRes);
                foreach (var propName in (isNew ? AllProperties : Properties))
                {
                    var prop = sRes.GetType().GetProperty(propName);
                    
                    switch (propName)
                    {
                        case "Availability":
                            if (sRes.NamedAvailability != null)
                            {
                                dRes.NamedAvailability = Destination.AvailabilityPatterns.FindByHostKey(sRes.NamedAvailability.HostKey);
                                if (dRes.NamedAvailability == null)
                                    throw new InvalidOperationException("Named availability not found: " + sRes.NamedAvailability.HostKey);
                            }
                            else
                            {
                                dRes.NamedAvailability = null;
                                dRes.BaseAvailability = new PeriodInYearPattern(Destination) { PatternAsArray = sRes.BaseAvailability.PatternAsArray };
                            }
                            break;

                        case "Suitabilities":
                            break;

                        case "AvoidConcurrency":
                            dynamic col = dRes.AvoidConcurrencyWith;
                            col.Clear();
                            foreach (var obj in sRes.AvoidConcurrencyWith)
                            {
                                var res = GetObject(Destination, obj.HostKey);
                                if (res == null)
                                    Log.WriteLine("Warning: concurrent location {0} not found", obj.HostKey);
                                else
                                    AddObject(dRes.AvoidConcurrencyWith, res);
                            }
                            break;

                        case "Department":
                            if (sRes.Department == null)
                                dRes.Department = null;
                            else
                            {
                                dRes.Department = Destination.Departments.FindByHostKey(sRes.Department.HostKey);
                                if (dRes.Department == null)
                                    throw new InvalidOperationException("Department not found: " + sRes.Department.HostKey);
                            }
                            break;

                        case "Zone":
                            if (sRes.Zone == null)
                                dRes.Zone = null;
                            else
                            {
                                dRes.Zone = Destination.Zones.FindByHostKey(sRes.Zone.HostKey);
                                if (dRes.Zone == null)
                                    throw new InvalidOperationException("Zone not found: " + sRes.Zone.HostKey);
                            }
                            break;

                        case "Tags":
                            CopyCollection(sRes.Tags, dRes.Tags, Destination.Tags);
                            break;

                        case "SharedWith":
                            CopyCollection(sRes.SharedWith, dRes.SharedWith, Destination.Departments);
                            break;

                        default:
                            if (prop == null)
                            {
                                if (!isNew)
                                    Log.WriteLine("Warning: property {0} does not exist",  propName);
                                continue;
                            }
                            var val = prop.GetValue(sRes);
                            prop.SetValue(dRes, val);
                            break;
                    }
                }
            }

            int count = Destination.Suitabilities.Count, cur = 0;
            Log.WriteLine();
            foreach (var dSuit in Destination.Suitabilities)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Log.WriteLine("Updating suitabilities: {0}/{1}", ++cur, count);
                var sSuit = Source.Suitabilities.FindByHostKey(dSuit.HostKey);
                if (sSuit == null)
                    continue;

                var sCol = TargetType == ResourceType.Staff ? (IEnumerable<IResourceObject>)sSuit.PrimaryStaff : sSuit.PrimaryLocations;
                var dCol = TargetType == ResourceType.Staff ? (IEnumerable<IResourceObject>)dSuit.PrimaryStaff : dSuit.PrimaryLocations;
                var sKeys = sCol.Select(l => l.HostKey).ToArray();
                var dKeys = dCol.Select(l => l.HostKey).ToArray();
                HostKeys.Where(l => sKeys.Contains(l) && !dKeys.Contains(l)).ForEach(l => AddObject(dCol, dict[l]));
                HostKeys.Where(l => !sKeys.Contains(l) && dKeys.Contains(l)).ForEach(l => RemoveObject(dCol, dict[l]));

                sCol = TargetType == ResourceType.Staff ? (IEnumerable<IResourceObject>)sSuit.OtherStaff : sSuit.OtherLocations;
                dCol = TargetType == ResourceType.Staff ? (IEnumerable<IResourceObject>)dSuit.OtherStaff : dSuit.OtherLocations;
                sKeys = sCol.Select(l => l.HostKey).ToArray();
                dKeys = dCol.Select(l => l.HostKey).ToArray();
                HostKeys.Where(l => sKeys.Contains(l) && !dKeys.Contains(l)).ForEach(l => AddObject(dCol, dict[l]));
                HostKeys.Where(l => !sKeys.Contains(l) && dKeys.Contains(l)).ForEach(l => RemoveObject(dCol, dict[l]));
            }
        }

        void CopyCollection<T>(SPlusCollection<T> from, SPlusCollection<T> to, SPlusCollection<T> toSource) where T : SPlusObject
        {
            to.Clear();
            foreach (var obj in from)
            {
                var t = toSource.FindByHostKey(obj.HostKey);
                if (t == null)
                {
                    Log.WriteLine("Warning: {0} {1} not found", typeof(T).Name, t.HostKey);
                    continue;
                }
                to.Add(t);
            }
        }

        IResourceObject GetObject(College c, string hostKey)
        {
            return TargetType == ResourceType.Staff ? (IResourceObject)c.StaffMembers.FindByHostKey(hostKey)
                                    : c.Locations.FindByHostKey(hostKey);
        }

        void AddObject(IEnumerable<IResourceObject> col, IResourceObject obj)
        {
            if (TargetType == ResourceType.Location)
                ((SPlusCollection<Location>)col).Add((Location)obj);
            else
                ((SPlusCollection<StaffMember>)col).Add((StaffMember)obj);
        }

        void RemoveObject(IEnumerable<IResourceObject> col, IResourceObject obj)
        {
            if (TargetType == ResourceType.Location)
                ((SPlusCollection<Location>)col).Remove((Location)obj);
            else
                ((SPlusCollection<StaffMember>)col).Remove((StaffMember)obj);
        }
    }
}
