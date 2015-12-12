using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.Utilities;

namespace UvA.SPlusTools.Data.Entities
{
    public abstract class Pattern
    {
        internal dynamic Object;
        public College College { get; set; }

        public bool[] PatternAsArray
        {
            get { return ((object[])Object.PatternAsArray).Cast<bool>().ToArray(); }
            set { Object.PatternAsArray = value.Cast<object>().ToArray(); }
        }

        protected int[] Values
        {
            get { var p = PatternAsArray; return Tools.Range(0, p.Length - 1).Where(r => p[r]).ToArray(); }
        }

        protected void SetPattern(int[] values, int length)
        {
            PatternAsArray = Tools.Range(0, length - 1).Select(r => values.Contains(r)).ToArray();
        }
    }

    public class PeriodInYearPattern : Pattern
    {
        public PeriodInYearPattern(College college, dynamic obj)
        {
            College = college;
            Object = obj;
        }

        public int[] Periods { get { return Values; } }

        public PeriodInYearPattern(College college)
        {
            College = college;
            Object = college.Object.CreatePeriodInYearPattern();
        }

        /// <summary>
        /// Sets the pattern to be a continuous range
        /// </summary>
        /// <param name="startPeriod">Starting period (inclusive)</param>
        /// <param name="endPeriod">End period (exclusive)</param>
        public void SetRange(int startPeriod, int endPeriod)
        {
            PatternAsArray = Tools.Range(0, College.PeriodsPerYear - 1).Select(p => p >= startPeriod && p < endPeriod).ToArray();
        }
    }

    public class WeekInYearPattern : Pattern
    {
        public int[] Weeks { get { return Values; } set { SetPattern(value, College.WeeksPerYear); } }

        public WeekInYearPattern(College college, dynamic obj)
        {
            College = college;
            Object = obj;
        }

        public WeekInYearPattern(College college)
        {
            College = college;
            Object = college.Object.CreateWeekInYearPattern();
        }

        /// <summary>
        /// Sets the pattern to be a continuous range
        /// </summary>
        /// <param name="startWeek">Starting week (inclusive)</param>
        /// <param name="endWeek">End week (inclusive)</param>
        public void SetRange(int startWeek, int endWeek)
        {
            PatternAsArray = Tools.Range(0, College.WeeksPerYear - 1).Select(p => p >= startWeek && p <= endWeek).ToArray();
        }
    }
}
