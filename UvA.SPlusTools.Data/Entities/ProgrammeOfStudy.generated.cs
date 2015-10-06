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


	}
}