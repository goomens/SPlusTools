using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.Utilities;

namespace UvA.SPlusTools.Data.Entities
{
    partial class Activity
    {
        public bool IsScheduled { get { return Object.SchedulingStatus == 0; } }

        public DayOfWeek[] SuggestedDaysInWeek
        {
            get { return ((object[])Object.SuggestedDaysInWeek).Select(r => College.GetDay((int)r)).ToArray(); }
            set { Object.SuggestedDaysInWeek = value.Select(r => r.ToSplusDay()).Cast<object>().ToArray(); }
        }

        public TimeSpan? SuggestedStartTime
        {
            get { return SuggestedPeriodInDay == -1 ? (TimeSpan?)null : College.PeriodToTime(SuggestedPeriodInDay); }
            set { SuggestedPeriodInDay = value == null ? -1 : College.TimeToPeriod(value.Value); }
        }

        IEnumerable<T> GetAllocatedResources<T>() where T : SPlusObject, IResourceObject
            => new SPlusCollection<T>(College, Object.GetResourceAllocation(ResourceRequirement<T>.ResourceIndex));

        public IEnumerable<Location> AllocatedLocations => IsScheduled ? GetAllocatedResources<Location>() : new Location[0]; 

        ResourceRequirement<Location> _LocationRequirement;
        /// <summary>
        /// Describes the location requirement of an Activity
        /// Note: changes are only written to S+ when the Set method is called
        /// </summary>
        public ResourceRequirement<Location> LocationRequirement { get { return _LocationRequirement = _LocationRequirement ?? new ResourceRequirement<Location>(this); } }

        ResourceRequirement<StaffMember> _StaffRequirement;
        /// <summary>
        /// Describes the staff requirement of an Activity
        /// Note: changes are only written to S+ when the Set method is called
        /// </summary>
        public ResourceRequirement<StaffMember> StaffRequirement { get { return _StaffRequirement = _StaffRequirement ?? new ResourceRequirement<StaffMember>(this); } }

        ResourceRequirement<StudentSet> _StudentSetRequirement;
        /// <summary>
        /// Describes the student set requirement of an Activity
        /// Note: changes are only written to S+ when the Set method is called
        /// </summary>
        public ResourceRequirement<StudentSet> StudentSetRequirement { get { return _StudentSetRequirement = _StudentSetRequirement ?? new ResourceRequirement<StudentSet>(this); } }

        public TimeSpan DurationTime { get { return TimeSpan.FromMinutes(Duration * College.PeriodLength); } set { Duration = (int)Math.Ceiling(value.TotalMinutes / College.PeriodLength); } }

        Dictionary<ResourceType, SPlusCollection<Suitability>> _Suitabilities;

        public SPlusCollection<Suitability> GetSuitabilities(ResourceType type)
        {
            if (_Suitabilities == null)
                _Suitabilities = new Dictionary<ResourceType, SPlusCollection<Suitability>>();
            if (!_Suitabilities.ContainsKey(type))
                _Suitabilities.Add(type, new SPlusCollection<Suitability>(College, Object.GetSuitabilities(type)));
            return _Suitabilities[type];
        }

        /// <summary>
        /// Required location suitabilities for this Activity
        /// Note: changes are only written to S+ when the SetSuitabilities method is called
        /// </summary>
        public SPlusCollection<Suitability> LocationSuitabilities => GetSuitabilities(ResourceType.Location);

        /// <summary>
        /// Required staff suitabilities for this Activity
        /// Note: changes are only written to S+ when the SetSuitabilities method is called
        /// </summary>
        public SPlusCollection<Suitability> StaffSuitabilities => GetSuitabilities(ResourceType.Staff);

        public void SaveSuitabilities(ResourceType type) => Object.SetSuitabilities(type, GetSuitabilities(type).Source);

        public void Reschedule()
            => Object.RescheduleMany();

        public void Unschedule()
            => Object.Unschedule();

        public bool Schedule()
        {
            Object.ScheduleMany();
            return IsScheduled;
        }

        public bool Schedule(PeriodInWeekPattern pattern)
        {
            Object.Schedule(pattern.Object);
            return IsScheduled;
        }

        /// <summary>
        /// Sets allocated week pattern and days. Unclear what this actually dues
        /// </summary>
        /// <param name="pat">Week pattern</param>
        /// <param name="days">Array of days</param>
        public void SetAllocatedDays(WeekInYearPattern pat, DayOfWeek[] days)
        {
            Object.SetAllocatedDays(pat.Object, days.Select(d => d.ToSplusDay()).Cast<object>().ToArray());
        }

        /// <summary>
        /// Sets the scheduled periods. Overrides any constraints
        /// </summary>
        /// <param name="dates">Dates to schedule the activity at</param>
        public void SetScheduledPeriods(DateTime[] dates)
        {
            SetScheduledPeriods(dates.Select(College.DateToPeriod).ToArray());
        }

        public void SetScheduledPeriods(int[] startPeriods)
        {
            Object.SetScheduledPeriods(startPeriods.Cast<object>().ToArray(), false);
        }

        public IEnumerable<DateTime> ScheduledStartDates => IsScheduled ? ((object[])Object.ScheduledStartDateTimes).Cast<DateTime>() : new DateTime[0];

        public DateTime StartDate
        {
            get => ScheduledStartDates.FirstOrDefault();
            set
            {
                SuggestedDaysInWeek = new DayOfWeek[] { value.DayOfWeek };
                WeekPattern = new WeekInYearPattern(College) { Weeks = new int[] { College.GetWeek(value) } };
            }
        }

        /// <summary>
        /// Creates a copy of the activity with all fields, including studentsets and staff/location suitabilities
        /// </summary>
        /// <param name="copyStaff">If <c>true</c>, include preset staff</param>
        /// <returns></returns>
        public Activity Copy(bool copyStaff = true)
        {
            var act = new Activity(College)
            {
                Name = Name,
                Description = Description,
                NamedAvailability = NamedAvailability,
                SuggestedDaysInWeek = SuggestedDaysInWeek,
                SuggestedStartTime = SuggestedStartTime,
                UserText1 = UserText1,
                UserText2 = UserText2,
                UserText3 = UserText3,
                UserText4 = UserText4,
                UserText5 = UserText5,
                Duration = Duration,
                ActivityType = ActivityType,
                Department = Department,
                Module = Module,
                PlannedSize = PlannedSize,
                Zone = Zone
            };
            StudentSets.ForEach(act.StudentSets.Add);
            LocationSuitabilities.ForEach(s => act.LocationSuitabilities.Add(s));
            act.SaveSuitabilities(ResourceType.Location);
            StaffSuitabilities.ForEach(s => act.StaffSuitabilities.Add(s));
            act.SaveSuitabilities(ResourceType.Staff);
            Tags.ForEach(act.Tags.Add);
            if (copyStaff)
            {
                StaffRequirement.Resources.ForEach(r => act.StaffRequirement.Resources.Add(r));
                act.StaffRequirement.Set(ResourceRequirementType.Preset);
            }
            return act;
        }

        #region Request booking

        public bool CancelRequest() => Object.CancelRequest();
        public bool DeclineRequest() => Object.DeclineRequest();
        public bool GrantRequest(string granter) => Object.GrantRequest(granter);
        public bool MakeARequest(int periodInWeekPattern, dynamic res, dynamic resourceTypesToSkip, string requester) => Object.MakeARequest(periodInWeekPattern, res, resourceTypesToSkip, requester);
        public bool ScheduleWithPossibleRequest(string requester, bool request) => Object.ScheduleWithPossibleRequest(requester, request);

        public string Requester => Object.Requester;
        public RequestStatus RequestStatus => (RequestStatus)(int)Object.RequestStatus;

        #endregion
    }
}
