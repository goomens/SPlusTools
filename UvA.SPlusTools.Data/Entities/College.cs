using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.SPlusTools.Data.Entities
{
    public partial class College : ITimeObject
    {
        internal dynamic Object { get; private set; }
        public int PeriodLength { get; private set; }

        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }


        Dictionary<string, SPlusObject> Objects;

        public College(string progID)
        {
            Object = ((dynamic)Activator.CreateInstance(Type.GetTypeFromProgID(progID + ".application"))).ActiveCollege;
            Objects = new Dictionary<string, SPlusObject>();
            StartTime = ((DateTime)Object.StartTime).TimeOfDay;
            EndTime = ((DateTime)Object.EndTime).TimeOfDay;
            if (EndTime.TotalMinutes == 0)
                EndTime = TimeSpan.FromDays(1);
            PeriodLength = (int)EndTime.Subtract(StartTime).TotalMinutes / PeriodsPerDay;
        }

        internal T GetObject<T>(dynamic obj) where T : SPlusObject
        {
            if (obj == null)
                return null;
            string id = obj.ObjectId;
            if (!Objects.ContainsKey(id))
                Objects.Add(obj.ObjectId, (T)Activator.CreateInstance(typeof(T), new object[] { this, obj }));
            return (T)Objects[id];
        }

        public Activity CreateActivity(DateTime date, double startTime, double endTime, Location loc)
        {
            Activity act = new Activity(this);

            return act;
        }
    }
}
