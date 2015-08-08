using System;
 

namespace UvA.SPlusTools.Data.Entities
{
	public partial class Suitability : SPlusObject
	{
		public Suitability(College col) 
		{
			College = col;
			Object = col.Object.CreateSuitability();
			College.GetObject<Suitability>(Object);
		}

		public Suitability(College col, dynamic obj) 
		{
			College = col;
			Object = obj;
		}

		SPlusCollection<Location> _PrimaryLocations;
		public SPlusCollection<Location> PrimaryLocations 
		{
			get 
			{
				if (_PrimaryLocations == null)
					_PrimaryLocations = new SPlusCollection<Location>(College, Object.PrimaryLocations); 
				return _PrimaryLocations; 
			}
		}
				SPlusCollection<StaffMember> _PrimaryStaff;
		public SPlusCollection<StaffMember> PrimaryStaff 
		{
			get 
			{
				if (_PrimaryStaff == null)
					_PrimaryStaff = new SPlusCollection<StaffMember>(College, Object.PrimaryStaff); 
				return _PrimaryStaff; 
			}
		}
				SPlusCollection<Location> _OtherLocations;
		public SPlusCollection<Location> OtherLocations 
		{
			get 
			{
				if (_OtherLocations == null)
					_OtherLocations = new SPlusCollection<Location>(College, Object.OtherLocations); 
				return _OtherLocations; 
			}
		}
				SPlusCollection<StaffMember> _OtherStaff;
		public SPlusCollection<StaffMember> OtherStaff 
		{
			get 
			{
				if (_OtherStaff == null)
					_OtherStaff = new SPlusCollection<StaffMember>(College, Object.OtherStaff); 
				return _OtherStaff; 
			}
		}
		
	}
} 