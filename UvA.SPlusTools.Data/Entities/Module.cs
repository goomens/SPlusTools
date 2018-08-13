using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UvA.SPlusTools.Data.Entities
{
    partial class Module
    {
        public SPlusCollection<Activity> GetActivities()
            => new SPlusCollection<Activity>(College, Object.ActivitiesAllocatedTo());
    }
}
