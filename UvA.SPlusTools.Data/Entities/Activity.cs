using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool Schedule()
        {
            Object.ScheduleMany();
            return IsScheduled;
        }

        public DateTime StartDate
        {
            get { throw new NotImplementedException(); }
            set
            {
                SuggestedDaysInWeek = new DayOfWeek[] { value.DayOfWeek };
                WeekPattern = new WeekInYearPattern(College) { Weeks = new int[] { College.GetWeek(value) } };
            }
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
