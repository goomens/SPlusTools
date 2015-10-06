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

		public int PlannedSize 
		{
			get { return Object.PlannedSize; }
			set { Object.PlannedSize = value; }
		}
				SPlusCollection<ProgrammeOfStudy> _ProgrammesOfStudyOptionalFor;
		public SPlusCollection<ProgrammeOfStudy> ProgrammesOfStudyOptionalFor 
		{
			get 
			{
				if (_ProgrammesOfStudyOptionalFor == null)
					_ProgrammesOfStudyOptionalFor = new SPlusCollection<ProgrammeOfStudy>(College, Object.ProgrammesOfStudyOptionalFor); 
				return _ProgrammesOfStudyOptionalFor; 
			}
		}
				SPlusCollection<ProgrammeOfStudy> _ProgrammesOfStudyMandatoryFor;
		public SPlusCollection<ProgrammeOfStudy> ProgrammesOfStudyMandatoryFor 
		{
			get 
			{
				if (_ProgrammesOfStudyMandatoryFor == null)
					_ProgrammesOfStudyMandatoryFor = new SPlusCollection<ProgrammeOfStudy>(College, Object.ProgrammesOfStudyMandatoryFor); 
				return _ProgrammesOfStudyMandatoryFor; 
			}
		}
		
	}
}