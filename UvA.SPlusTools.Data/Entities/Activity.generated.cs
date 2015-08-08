using System;
 

namespace UvA.SPlusTools.Data.Entities
{
	public partial class Activity : SPlusObject
	{
		public Activity(College col) 
		{
			College = col;
			Object = col.Object.CreateActivity();
			College.GetObject<Activity>(Object);
		}

		public Activity(College col, dynamic obj) 
		{
			College = col;
			Object = obj;
		}


		ActivityType _ActivityType;
		public ActivityType ActivityType 
		{
			get 
			{
				if (_ActivityType == null)
									_ActivityType = College.GetObject<ActivityType>(Object.ActivityType); 
                				return _ActivityType; 
			}
			set 
			{
			    _ActivityType = value;
				Object.ActivityType = value == null ? null : value.Object;
			}
		}
		
		Zone _Zone;
		public Zone Zone 
		{
			get 
			{
				if (_Zone == null)
									_Zone = College.GetObject<Zone>(Object.Zone); 
                				return _Zone; 
			}
			set 
			{
			    _Zone = value;
				Object.Zone = value == null ? null : value.Object;
			}
		}
		
		AvailabilityPattern _NamedAvailability;
		public AvailabilityPattern NamedAvailability 
		{
			get 
			{
				if (_NamedAvailability == null)
									_NamedAvailability = College.GetObject<AvailabilityPattern>(Object.AvailabilityPattern); 
                				return _NamedAvailability; 
			}
			set 
			{
			    _NamedAvailability = value;
				Object.AvailabilityPattern = value == null ? null : value.Object;
			}
		}
		
		PeriodInYearPattern _BaseAvailability;
		public PeriodInYearPattern BaseAvailability 
		{
			get 
			{
				if (_BaseAvailability == null)
									_BaseAvailability = new PeriodInYearPattern(College, Object.BaseAvailability); 
								return _BaseAvailability; 
			}
			set 
			{
			    _BaseAvailability = value;
				Object.BaseAvailability = value == null ? null : value.Object;
			}
		}
				public int Duration 
		{
			get { return Object.Duration; }
			set { Object.Duration = value; }
		}
				public int SuggestedPeriodInDay 
		{
			get { return Object.SuggestedPeriodInDay; }
			set { Object.SuggestedPeriodInDay = value; }
		}
		
		Module _Module;
		public Module Module 
		{
			get 
			{
				if (_Module == null)
									_Module = College.GetObject<Module>(Object.Module); 
                				return _Module; 
			}
			set 
			{
			    _Module = value;
				Object.Module = value == null ? null : value.Object;
			}
		}
				public int PlannedSize 
		{
			get { return Object.PlannedSize; }
			set { Object.PlannedSize = value; }
		}
		
		WeekInYearPattern _WeekPattern;
		public WeekInYearPattern WeekPattern 
		{
			get 
			{
				if (_WeekPattern == null)
									_WeekPattern = new WeekInYearPattern(College, Object.WeekPattern); 
								return _WeekPattern; 
			}
			set 
			{
			    _WeekPattern = value;
				Object.WeekPattern = value == null ? null : value.Object;
			}
		}
		
	}
}