using System;
 

namespace UvA.SPlusTools.Data.Entities
{
	public partial class Department : SPlusObject
	{
		public Department(College col) 
		{
			College = col;
			Object = col.Object.CreateDepartment();
			College.GetObject<Department>(Object);
		}

		public Department(College col, dynamic obj) 
		{
			College = col;
			Object = obj;
		}


	}
}