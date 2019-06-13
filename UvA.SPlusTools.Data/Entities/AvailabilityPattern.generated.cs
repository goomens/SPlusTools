using System;
 

namespace UvA.SPlusTools.Data.Entities
{
	public partial class AvailabilityPattern : SPlusObject
	{
		public AvailabilityPattern(College col) 
		{
			College = col;
			Object = col.Object.CreateAvailabilityPattern();
			College.GetObject<AvailabilityPattern>(Object);
		}

		public AvailabilityPattern(College col, dynamic obj) 
		{
			College = col;
			Object = obj;
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