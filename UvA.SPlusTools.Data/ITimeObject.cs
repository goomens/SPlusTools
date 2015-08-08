using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.SPlusTools.Data
{
    public interface ITimeObject
    {
        TimeSpan StartTime { get; }
        int PeriodLength { get; }
        DateTime StartDate { get; }
        int PeriodsPerDay { get; }
    }

    public static class TimeObjectExtensions
    {
        public static int TimeToPeriod(this ITimeObject o, TimeSpan time)
        {
            return (int)(time.Subtract(o.StartTime).TotalMinutes / o.PeriodLength);
        }

        public static TimeSpan PeriodToTime(this ITimeObject o, int periodInDay)
        {
            return o.StartTime.Add(TimeSpan.FromMinutes(periodInDay * o.PeriodLength));
        }

        public static int GetWeek(this ITimeObject o, DateTime date)
        {
            return (int)Math.Ceiling((date.Date.Subtract(o.StartDate).TotalDays + 1) / 7.0) - 1;
        }

        public static int ToSplusDay(this DayOfWeek day)
        {
            return day == DayOfWeek.Sunday ? 6 : ((int)day - 1);
        }

        public static DayOfWeek GetDay(this ITimeObject o, int day)
        {
            return (DayOfWeek)((day + 1) % 7);
        }

        public static int DateToPeriod(this ITimeObject o, DateTime date)
        {
            return (int)Math.Floor(date.Subtract(o.StartDate).TotalDays) * o.PeriodsPerDay + (date.TimeOfDay == TimeSpan.FromDays(0) ? 0 : o.TimeToPeriod(date.TimeOfDay));
        }

        /// <summary>
        /// Converts a date to a period in year index
        /// </summary>
        /// <param name="o"></param>
        /// <param name="week">Zero-indexed week</param>
        /// <param name="day">S+ day</param>
        /// <returns></returns>
        public static int DateToPeriod(this ITimeObject o, int week, int day)
        {
            return o.PeriodsPerDay * 7 * week + day * o.PeriodsPerDay;
        }
    }
}
