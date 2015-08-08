using System;
 

namespace UvA.SPlusTools.Data.Entities
{
	public partial class ActivityGroup : SPlusObject
	{
		public ActivityGroup(College col) 
		{
			College = col;
			Object = col.Object.CreateActivityGroup();
			College.GetObject<ActivityGroup>(Object);
		}

		public ActivityGroup(College col, dynamic obj) 
		{
			College = col;
			Object = obj;
		}

		SPlusCollection<Activity> _Members;
		public SPlusCollection<Activity> Members 
		{
			get 
			{
				if (_Members == null)
					_Members = new SPlusCollection<Activity>(College, Object.Members); 
				return _Members; 
			}
		}
		
	}
} 