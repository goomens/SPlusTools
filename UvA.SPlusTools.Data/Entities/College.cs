using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.Utilities;

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

        /// <summary>
        /// Creates a PeriodInYearPattern based on the specified values
        /// </summary>
        /// <param name="startWeek">Starting week, zero-indexed</param>
        /// <param name="endWeek">End week, inclusive</param>
        /// <returns></returns>
        public PeriodInYearPattern GeneratePeriodInYearPattern(int startWeek, int endWeek)
        {
            var pat = new PeriodInYearPattern(this);
            pat.SetRange(startWeek * PeriodsPerDay * 7, endWeek * PeriodsPerDay * 7);
            return pat;
        }

        public bool IsConnected => Object.Application.SDB.IsConnected;
        public void Connect() => Object.Application.SDB.Connect();
        public void Disconnect() => Object.Application.SDB.Disconnect();

        public void Save(string path) => Object.Application.SaveImage(path);

        public void Writeback()
        {
            if (!Object.Application.SDB.IsConnected)
                Object.Application.SDB.Connect();
            Object.Application.SDB.WriteBack();
        }

        /// <summary>
        /// Creates a WeekInYearPattern based on the specified values
        /// </summary>
        /// <param name="startWeek">Starting week, zero-indexed</param>
        /// <param name="endWeek">End week, inclusive</param>
        /// <returns></returns>
        public WeekInYearPattern GenerateWeekInYearPattern(int startWeek, int endWeek)
        {
            var pat = new WeekInYearPattern(this);
            pat.SetRange(startWeek, endWeek);
            return pat;
        }
    }
}
