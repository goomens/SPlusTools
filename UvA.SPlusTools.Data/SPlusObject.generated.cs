using UvA.SPlusTools.Data.Entities;
using System;
 

namespace UvA.SPlusTools.Data
{
    public abstract partial class SPlusObject
    {

		Department _Department;
		public Department Department 
		{
			get 
			{
				if (_Department == null)
									_Department = College.GetObject<Department>(Object.Department); 
                				return _Department; 
			}
			set 
			{
			    _Department = value;
				Object.Department = value == null ? null : value.Object;
			}
		}
				SPlusCollection<Tag> _Tags;
		public SPlusCollection<Tag> Tags 
		{
			get 
			{
				if (_Tags == null)
					_Tags = new SPlusCollection<Tag>(College, Object.AssociatedTags); 
				return _Tags; 
			}
		}
		
	}
}