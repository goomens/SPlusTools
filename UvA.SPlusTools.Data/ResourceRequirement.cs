using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.SPlusTools.Data.Entities;

namespace UvA.SPlusTools.Data
{
    public enum ResourceRequirementType { None = 0 , Preset = 1, Wildcard = 2 }

    public class ResourceRequirement<T> where T : SPlusObject, IResourceObject
    {
        Activity Activity;

        public ResourceRequirement(Activity obj)
        {
            Activity = obj;
        }

        public ResourceRequirementType Type 
        {
            get { return (ResourceRequirementType)Activity.Object.GetResourceRequirementType(ResourceIndex); }
        }

        public int Number
        {
            get { return Activity.Object.GetResourceRequirementNumber(ResourceIndex); }
        }

        SPlusCollection<T> _Resources;
        public SPlusCollection<T> Resources
        {
            get
            {
                if (_Resources == null)
                    _Resources = new SPlusCollection<T>(Activity.College, Activity.Object.GetResourceRequirementPresets(ResourceIndex));
                return _Resources;
            }
        }
        
        public void Set(ResourceRequirementType type, int number = 0)
        {
            if (type == ResourceRequirementType.None)
                Activity.Object.SetResourceRequirement(ResourceIndex, (int)type);
            else if (type == ResourceRequirementType.Wildcard)
                Activity.Object.SetResourceRequirement(ResourceIndex, (int)type, number);
            else
                Activity.Object.SetResourceRequirement(ResourceIndex, (int)type, Resources.Count, Resources.Source);
        }

        int ResourceIndex
        {
            get
            {
                // 2 = equipment
                if (typeof(T) == typeof(StudentSet))
                    return 3;
                else if (typeof(T) == typeof(Location))
                    return 1;
                else if (typeof(T) == typeof(StaffMember))
                    return 0;
                throw new ArgumentException();
            }
        }
    }
}
