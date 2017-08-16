using System;
 

namespace UvA.SPlusTools.Data.Entities
{
	public partial class ProgrammeOfStudy : SPlusObject
	{
		public ProgrammeOfStudy(College col) 
		{
			College = col;
			Object = col.Object.CreateProgrammeOfStudy();
			College.GetObject<ProgrammeOfStudy>(Object);
		}

		public ProgrammeOfStudy(College col, dynamic obj) 
		{
			College = col;
			Object = obj;
		}

		SPlusCollection<Module> _MandatoryModules;
		public SPlusCollection<Module> MandatoryModules 
		{
			get 
			{
				if (_MandatoryModules == null)
					_MandatoryModules = new SPlusCollection<Module>(College, Object.MandatoryModules); 
				return _MandatoryModules; 
			}
		}
				SPlusCollection<Module> _OptionalModules;
		public SPlusCollection<Module> OptionalModules 
		{
			get 
			{
				if (_OptionalModules == null)
					_OptionalModules = new SPlusCollection<Module>(College, Object.OptionalModules); 
				return _OptionalModules; 
			}
		}
		
	}
}