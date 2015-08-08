using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.SPlusTools.Data.Booking
{
    public class LocationInfo
    {
        public string ObjectId { get; set; }
        public string Name { get; set; }
        public string HostKey { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }
        public ZoneInfo Zone { get; set; }
        public string Availability { get; set; }
        public ICollection<SuitabilityInfo> Suitabilities { get; set; }
        public ICollection<LocationInfo> AvoidConcurrencyWith { get; set; }

        public LocationInfo()
        {
            Suitabilities = new List<SuitabilityInfo>();
            AvoidConcurrencyWith = new List<LocationInfo>();
        }

        public bool IsAvailable(int startPeriod, int endPeriod)
        {
            return Availability.Substring(startPeriod, endPeriod - startPeriod).All(c => c == '1');
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
