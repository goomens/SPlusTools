using System;
 

namespace UvA.SPlusTools.Data.Entities
{
	public partial class Zone : SPlusObject
	{
		public Zone(College col) 
		{
			College = col;
			Object = col.Object.CreateZone();
			College.GetObject<Zone>(Object);
		}

		public Zone(College col, dynamic obj) 
		{
			College = col;
			Object = obj;
		}


	}
}