using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.SPlusTools.Data.Entities
{
    public interface IResourceObject : ISPlusObject
    {
        PeriodInYearPattern BaseAvailability { get; set; }
        AvailabilityPattern NamedAvailability { get; set; }
        Zone Zone { get; set; }
        IEnumerable<IResourceObject> AvoidConcurrencyWith { get; }
        SPlusCollection<Department> SharedWith { get; }
    }

    public interface ISPlusObject
    {
        string Name { get; set; }
        string Description { get; set; }
        string HostKey { get; set; }
        string UserText1 { get; set; }
        string UserText2 { get; set; }
        string UserText3 { get; set; }
        string UserText4 { get; set; }
        string UserText5 { get; set; }

        Department Department { get; set; }
        SPlusCollection<Tag> Tags { get; }
    }

    public partial class StaffMember : IResourceObject
    {
        IEnumerable<IResourceObject> IResourceObject.AvoidConcurrencyWith
        {
            get
            {
                return AvoidConcurrencyWith;
            }
        }
    }

    public partial class StudentSet : IResourceObject
    {
        IEnumerable<IResourceObject> IResourceObject.AvoidConcurrencyWith
        {
            get
            {
                return AvoidConcurrencyWith;
            }
        }
    }

    public partial class Location : IResourceObject
    {
        IEnumerable<IResourceObject> IResourceObject.AvoidConcurrencyWith
        {
            get
            {
                return AvoidConcurrencyWith;
            }
        }
    }
}
