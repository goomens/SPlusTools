using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.SPlusTools.Data.Entities;

namespace UvA.SPlusTools.Data.Tasks
{
    public class HandleRequestsTask : ImageTask
    {
        dynamic College;
        int Progress;

        public HandleRequestsTask()
        {

        }

        public string LocationDepartmentFilter { get; set; }
        
        public override void Execute()
        {
            College = new College(ProgID).Object;

            double total = College.Activities.Count; double count = 0;
            var setting = College.CreateSchedulingResourceSetting();
            setting.UseLocation = true;
            foreach (var act in College.Activities)
            {
                Progress = (int)Math.Round(100.0 * ++count / total);
                if (act.IsRequest)
                {
                    if (LocationDepartmentFilter != null)
                    {
                        var locs = act.GetResourceRequirement(1);
                        foreach (var loc in locs)
                            if (loc.Department.Name != LocationDepartmentFilter)
                                continue;
                    }
                    act.ScheduleSpecialMany(setting);
                }
            }
        }
    }
}
