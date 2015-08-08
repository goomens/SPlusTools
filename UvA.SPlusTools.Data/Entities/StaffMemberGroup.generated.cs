using System;
 

namespace UvA.SPlusTools.Data.Entities
{
	public partial class StaffMemberGroup : SPlusObject
	{
		public StaffMemberGroup(College col) 
		{
			College = col;
			Object = col.Object.CreateStaffMemberGroup();
			College.GetObject<StaffMemberGroup>(Object);
		}

		public StaffMemberGroup(College col, dynamic obj) 
		{
			College = col;
			Object = obj;
		}

		SPlusCollection<StaffMember> _Members;
		public SPlusCollection<StaffMember> Members 
		{
			get 
			{
				if (_Members == null)
					_Members = new SPlusCollection<StaffMember>(College, Object.Members); 
				return _Members; 
			}
		}
		
	}
} 