using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.SPlusTools.Data.Entities;

namespace UvA.SPlusTools.Data.Tasks
{
    public enum ActionType { Add, Remove }

    /// <summary>
    /// Add or remove a tag to all activities in a group
    /// </summary>
    public class EditTagsTask : ImageTask
    {
        College College;

        public string ActivityGroupName { get; set; }
        public string TagName { get; set; }
        public ActionType Action { get; set; }

        public override void Execute()
        {
            College = new College(ProgID);

            var tag = College.Tags.FindByName(TagName);
            var group = College.ActivityGroups.FindByName(ActivityGroupName);
            foreach (var member in group.Members)
            {
                if (Action == ActionType.Add)
                    member.Tags.Add(tag);
                else
                    member.Tags.Remove(tag);
            }
        }
    }
}
