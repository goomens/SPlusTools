using System;
 

namespace UvA.SPlusTools.Data.Entities
{
	public partial class Module : SPlusObject
	{
		public Module(College col) 
		{
			College = col;
			Object = col.Object.CreateModule();
			College.GetObject<Module>(Object);
		}

		public Module(College col, dynamic obj) 
		{
			College = col;
			Object = obj;
		}


	}
}