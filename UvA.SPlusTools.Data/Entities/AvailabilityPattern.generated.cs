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


	}
}