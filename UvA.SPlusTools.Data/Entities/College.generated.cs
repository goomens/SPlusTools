using UvA.SPlusTools.Data.Entities;
using System;
 

namespace UvA.SPlusTools.Data.Entities
{
    public partial class College
    {
		public DateTime StartDate 
		{
			get { return Object.StartDate; }
			set { Object.StartDate = value; }
		}
				public int PeriodsPerDay 
		{
			get { return Object.PeriodsPerDay; }
			set { Object.PeriodsPerDay = value; }
		}
				public int PeriodsPerYear 
		{
			get { return Object.PeriodsPerYear; }
			set { Object.PeriodsPerYear = value; }
		}
				public int WeeksPerYear 
		{
			get { return Object.WeeksPerYear; }
			set { Object.WeeksPerYear = value; }
		}
				SPlusCollection<Activity> _Activities;
		public SPlusCollection<Activity> Activities 
		{
			get 
			{
				if (_Activities == null)
					_Activities = new SPlusCollection<Activity>(this, Object.Activities); 
				return _Activities; 
			}
		}
				SPlusCollection<Module> _Modules;
		public SPlusCollection<Module> Modules 
		{
			get 
			{
				if (_Modules == null)
					_Modules = new SPlusCollection<Module>(this, Object.Modules); 
				return _Modules; 
			}
		}
				SPlusCollection<ActivityGroup> _ActivityGroups;
		public SPlusCollection<ActivityGroup> ActivityGroups 
		{
			get 
			{
				if (_ActivityGroups == null)
					_ActivityGroups = new SPlusCollection<ActivityGroup>(this, Object.ActivityGroups); 
				return _ActivityGroups; 
			}
		}
				SPlusCollection<ActivityType> _ActivityTypes;
		public SPlusCollection<ActivityType> ActivityTypes 
		{
			get 
			{
				if (_ActivityTypes == null)
					_ActivityTypes = new SPlusCollection<ActivityType>(this, Object.ActivityTypes); 
				return _ActivityTypes; 
			}
		}
				SPlusCollection<Department> _Departments;
		public SPlusCollection<Department> Departments 
		{
			get 
			{
				if (_Departments == null)
					_Departments = new SPlusCollection<Department>(this, Object.Departments); 
				return _Departments; 
			}
		}
				SPlusCollection<StaffMember> _StaffMembers;
		public SPlusCollection<StaffMember> StaffMembers 
		{
			get 
			{
				if (_StaffMembers == null)
					_StaffMembers = new SPlusCollection<StaffMember>(this, Object.StaffMembers); 
				return _StaffMembers; 
			}
		}
				SPlusCollection<Location> _Locations;
		public SPlusCollection<Location> Locations 
		{
			get 
			{
				if (_Locations == null)
					_Locations = new SPlusCollection<Location>(this, Object.Locations); 
				return _Locations; 
			}
		}
				SPlusCollection<StudentSet> _StudentSets;
		public SPlusCollection<StudentSet> StudentSets 
		{
			get 
			{
				if (_StudentSets == null)
					_StudentSets = new SPlusCollection<StudentSet>(this, Object.StudentSets); 
				return _StudentSets; 
			}
		}
				SPlusCollection<Suitability> _Suitabilities;
		public SPlusCollection<Suitability> Suitabilities 
		{
			get 
			{
				if (_Suitabilities == null)
					_Suitabilities = new SPlusCollection<Suitability>(this, Object.Suitabilities); 
				return _Suitabilities; 
			}
		}
				SPlusCollection<Zone> _Zones;
		public SPlusCollection<Zone> Zones 
		{
			get 
			{
				if (_Zones == null)
					_Zones = new SPlusCollection<Zone>(this, Object.Zones); 
				return _Zones; 
			}
		}
				SPlusCollection<AvailabilityPattern> _AvailabilityPatterns;
		public SPlusCollection<AvailabilityPattern> AvailabilityPatterns 
		{
			get 
			{
				if (_AvailabilityPatterns == null)
					_AvailabilityPatterns = new SPlusCollection<AvailabilityPattern>(this, Object.AvailabilityPatterns); 
				return _AvailabilityPatterns; 
			}
		}
				SPlusCollection<Tag> _Tags;
		public SPlusCollection<Tag> Tags 
		{
			get 
			{
				if (_Tags == null)
					_Tags = new SPlusCollection<Tag>(this, Object.Tags); 
				return _Tags; 
			}
		}
				SPlusCollection<ProgrammeOfStudy> _ProgrammesOfStudy;
		public SPlusCollection<ProgrammeOfStudy> ProgrammesOfStudy 
		{
			get 
			{
				if (_ProgrammesOfStudy == null)
					_ProgrammesOfStudy = new SPlusCollection<ProgrammeOfStudy>(this, Object.ProgrammesOfStudy); 
				return _ProgrammesOfStudy; 
			}
		}
		
	}
}