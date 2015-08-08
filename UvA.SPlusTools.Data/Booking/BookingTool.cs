using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.Utilities;

namespace UvA.SPlusTools.Data.Booking
{
    public class BookingTool : QueryTool, ITimeObject
    {
        public int PeriodsPerDay { get; set; }
        public int WeeksPerYear { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int PeriodLength { get; private set; }
        public DateTime StartDate { get; set; }

        public List<ZoneInfo> Zones { get; private set; }
        public List<LocationInfo> Locations { get; private set; }
        public List<SuitabilityInfo> Suitabilities { get; private set; }

        string ZoneGroupName = "WRB";
        string LocationGroupName = "WRB";
        string SuitabilityGroupName = "WRB";

        public BookingTool(string progID)
        {
            ProgID = progID;

            var row = DoQuery("SELECT PeriodsPerDay, WeeksPerYear, StartTime, EndTime, StartDate FROM College").Single();
            PeriodsPerDay = (int)row[0];
            WeeksPerYear = (int)row[1];
            StartTime = (TimeSpan)row[2];
            EndTime = (TimeSpan)row[3];
            if (EndTime.TotalMinutes == 0)
                EndTime = TimeSpan.FromDays(1);
            StartDate = (DateTime)row[4];
            PeriodLength = (int)EndTime.Subtract(StartTime).TotalMinutes / PeriodsPerDay;

            LoadZones();
            LoadLocations();
            LoadLocationConcurrency();
            LoadSuitabilities();
        }

        public IEnumerable<LocationInfo> GetAvailableLocations(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            return GetAvailableLocations(this.TimeToPeriod(startTime), this.TimeToPeriod(endTime), date.DayOfWeek.ToSplusDay(), this.GetWeek(date));
        }

        public IEnumerable<LocationInfo> GetAvailableLocations(int startPeriod, int endPeriod, int day, int week)
        {
            var acts = DoQuery(string.Format(@"SELECT Activity_ObjectID FROM Activity
WHERE ScheduledDaysInWeekAsString LIKE '%{0}%' and ScheduledPeriodInDay < {2} and ScheduledPeriodInDay + Duration > {1} and ScheduledWeeks_PatternAsArray LIKE '{3}1%'",
                day, startPeriod, endPeriod, UvA.Utilities.Tools.Range(0, week).Skip(1).ToSeparatedString(s => "_", ""))).Select(r => r[0].ToString());

            var rels = GetField("Location_ObjectId", "Location_ActivitiesAllocatedTo", Parameter("Activity_ObjectId", acts));
            var locs = Locations.Where(l => l.IsAvailable(this.DateToPeriod(week, day) + startPeriod, this.DateToPeriod(week, day) + endPeriod)
                && !rels.Contains(l.ObjectId) && !l.AvoidConcurrencyWith.Any(z => rels.Contains(z.ObjectId)));
            //var locs = DoQuery(@"SELECT Name From Location WHERE NOT Location_ObjectId IN (" +
            //    rels.Select(r => r[0].ToString()).Distinct().ToSeparatedString(r => "'" + r + "'")
            //    + ")").Select(r => r[0].ToString()).ToArray();
            return locs;
        }

        void LoadZones()
        {
            var groupID = GetSingleField("ZoneGroup_ObjectId", "ZoneGroup", Parameter("Name", ZoneGroupName));
            var zoneIDs = GetField("Zone_ObjectId", "ZoneGroup_Members", Parameter("ZoneGroup_ObjectId", groupID));
            var zones = DoQuery("Zone_ObjectId, Name, Description, Containing_Zone_ObjectId", "Zone");
            Zones = zones.Select(r => new ZoneInfo()
                {
                    ObjectId = r[0].ToString(),
                    Name = r[1].ToString(),
                    Description = r[2].ToString(),
                    IsBookEnabled = zoneIDs.Contains(r[0].ToString())
                }).ToList();
            zones.Where(r => !(r[3] is DBNull)).ForEach(r => Zones.First(z => z.ObjectId == r[0].ToString()).Zone = Zones.FirstOrDefault(z => z.ObjectId == r[3].ToString()));
        }

        void LoadLocations()
        {
            var groupID = GetSingleField("LocationGroup_ObjectId", "LocationGroup", Parameter("Name", LocationGroupName));
            var locIDs = GetField("Location_ObjectId", "LocationGroup_Members", Parameter("LocationGroup_ObjectId", groupID));
            var locs = DoQuery("Location_ObjectId, Name, Description, Zone_ObjectId, BaseAvailability_PatternAsArray, Capacity, HostKey", "Location"); // , Parameter("Location_ObjectId", locIDs)
            Locations = locs.Select(r => new LocationInfo()
            {
                ObjectId = r[0].ToString(),
                Name = r[1].ToString(),
                Description = r[2].ToString(),
                Zone = Zones.FirstOrDefault(z => z.ObjectId == r[3].ToString()),
                Availability = r[4].ToString(),
                Capacity = (int)r[5],
                HostKey = r[6].ToString()
            }).ToList();
        }

        void LoadLocationConcurrency()
        {
            var rels = DoQuery("*", "Location_AvoidConcurrencyWith");
            rels.GroupBy(r => r[0].ToString())
                .ForEach(g =>
                {
                    var loc = Locations.First(l => l.ObjectId == g.Key);
                    var keys = g.Select(z => z[1].ToString());
                    loc.AvoidConcurrencyWith = Locations.Where(l => keys.Contains(l.ObjectId)).ToArray();
                });
        }

        void LoadSuitabilities()
        {
            var groupID = GetSingleField("SuitabilityGroup_ObjectId", "SuitabilityGroup", Parameter("Name", SuitabilityGroupName));
            var suitIDs = GetField("Suitability_ObjectId", "SuitabilityGroup_Members", Parameter("SuitabilityGroup_ObjectId", groupID));
            var suits = DoQuery("Suitability_ObjectId, Name, Description, Locations", "Suitability", Parameter("Suitability_ObjectId", suitIDs));
            Suitabilities = suits.Select(r => new SuitabilityInfo()
            {
                ObjectId = r[0].ToString(),
                Name = r[1].ToString(),
                Description = r[2].ToString()
            }).ToList();
            var rels = DoQuery("*", "Suitability_PrimaryLocations", Parameter("Suitability_ObjectId", suitIDs));
            rels.GroupBy(r => r["Location_ObjectId"].ToString()).ForEach(g =>
                {
                    var loc = Locations.FirstOrDefault(l => l.ObjectId == g.Key);
                    var relIDs = g.Select(r => r["Suitability_ObjectId"].ToString()).ToArray();
                    if (loc != null)
                        loc.Suitabilities = Suitabilities.Where(s => relIDs.Contains(s.ObjectId)).ToList();
                });
        }
    }
}
