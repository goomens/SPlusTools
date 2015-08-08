using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.SPlusTools.Data.Booking
{
    public class ZoneInfo
    {
        public string ObjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ZoneInfo Zone { get; set; }
        public bool IsBookEnabled { get; set; }

        public bool InheritsFrom(string zoneId)
        {
            return zoneId == ObjectId || (Zone != null && Zone.InheritsFrom(zoneId));
        }

        public override string ToString()
        {
            return Description;
        }
    }
}
