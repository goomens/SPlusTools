using System;
 

namespace UvA.SPlusTools.Data.Entities
{
	public partial class Location : SPlusObject
	{
		public Location(College col) 
		{
			College = col;
			Object = col.Object.CreateLocation();
			College.GetObject<Location>(Object);
		}

		public Location(College col, dynamic obj) 
		{
			College = col;
			Object = obj;
		}

		public int Capacity 
		{
			get { return Object.Capacity; }
			set { Object.Capacity = value; }
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
				SPlusCollection<Location> _AvoidConcurrencyWith;
		public SPlusCollection<Location> AvoidConcurrencyWith 
		{
			get 
			{
				if (_AvoidConcurrencyWith == null)
					_AvoidConcurrencyWith = new SPlusCollection<Location>(College, Object.AvoidConcurrencyWith); 
				return _AvoidConcurrencyWith; 
			}
		}
				SPlusCollection<Department> _SharedWith;
		public SPlusCollection<Department> SharedWith 
		{
			get 
			{
				if (_SharedWith == null)
					_SharedWith = new SPlusCollection<Department>(College, Object.SharedWithDepartments); 
				return _SharedWith; 
			}
		}
		
	}
}