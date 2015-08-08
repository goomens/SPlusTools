using System;
 

namespace UvA.SPlusTools.Data.Entities
{
	public partial class ActivityType : SPlusObject
	{
		public ActivityType(College col) 
		{
			College = col;
			Object = col.Object.CreateActivityType();
			College.GetObject<ActivityType>(Object);
		}

		public ActivityType(College col, dynamic obj) 
		{
			College = col;
			Object = obj;
		}


	}
}