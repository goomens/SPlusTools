using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.SPlusTools.Data.Entities;

namespace UvA.SPlusTools.Data
{
    public enum ResourceRequirementType { None = 0 , Preset = 1, Wildcard = 2 }

    /// <summary>
    /// Describes the resource requirement of an Activity for resources of type T
    /// Note: changes are only written to S+ when the Set method is called
    /// </summary>
    /// <typeparam name="T">Resource type</typeparam>
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
        /// <summary>
        /// The required resources, ignored if type is not Preset
        /// </summary>
        public SPlusCollection<T> Resources
        {
            get
            {
                if (_Resources == null)
                    _Resources = new SPlusCollection<T>(Activity.College, Activity.Object.GetResourceRequirementPresets(ResourceIndex));
                return _Resources;
            }
        }
        
        /// <summary>
        /// Writes the resource info to S+
        /// </summary>
        /// <param name="type">Requirement type</param>
        /// <param name="number">Number of suitabilitiies, ignored if type is not Wildcard</param>
        public void Set(ResourceRequirementType type, int number = 0)
        {
            if (type == ResourceRequirementType.None)
                Activity.Object.SetResourceRequirement(ResourceIndex, (int)type);
            else if (type == ResourceRequirementType.Wildcard)
                Activity.Object.SetResourceRequirement(ResourceIndex, (int)type, number);
            else
                Activity.Object.SetResourceRequirement(ResourceIndex, (int)type, Resources.Count, Resources.Source);
        }

        internal static int ResourceIndex
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
