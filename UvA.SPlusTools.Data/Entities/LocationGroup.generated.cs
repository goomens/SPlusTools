using System;
 

namespace UvA.SPlusTools.Data.Entities
{
	public partial class LocationGroup : SPlusObject
	{
		public LocationGroup(College col) 
		{
			College = col;
			Object = col.Object.CreateLocationGroup();
			College.GetObject<LocationGroup>(Object);
		}

		public LocationGroup(College col, dynamic obj) 
		{
			College = col;
			Object = obj;
		}

		SPlusCollection<Location> _Members;
		public SPlusCollection<Location> Members 
		{
			get 
			{
				if (_Members == null)
					_Members = new SPlusCollection<Location>(College, Object.Members); 
				return _Members; 
			}
		}
		
	}
} 