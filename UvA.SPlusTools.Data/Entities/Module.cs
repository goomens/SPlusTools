using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.SPlusTools.Data.Entities
{
    partial class Module
    {
        public SPlusCollection<Activity> GetAllocatedActivities()
            => new SPlusCollection<Activity>(College, Object.ActivitiesAllocatedTo());

        public SPlusCollection<Activity> GetLinkedActivities()
            => new SPlusCollection<Activity>(College, Object.ActivitiesLinkedTo());
    }
}
