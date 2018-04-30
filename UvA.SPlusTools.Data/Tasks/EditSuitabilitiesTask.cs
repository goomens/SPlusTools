using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UvA.SPlusTools.Data.Entities;

namespace UvA.SPlusTools.Data.Tasks
{
    /// <summary>
    /// Add or remove a tag to all activities in a group
    /// </summary>
    public class EditSuitabilitiesTask : ImageTask
    {
        College College;

        public string ActivityGroupName { get; set; }
        public string SuitabilityName { get; set; }
        public ActionType Action { get; set; }
        public ResourceType ResourceType { get; set; } = ResourceType.Location;

        public override void Execute()
        {
            College = new College(ProgID);

            var suit = College.Suitabilities.FindByName(SuitabilityName);
            var group = College.ActivityGroups.FindByName(ActivityGroupName);
            foreach (var member in group.Members)
            {
                var suits = member.GetSuitabilities(ResourceType);
                if (Action == ActionType.Add)
                    suits.Add(suit);
                else
                    suits.Remove(suit);
                member.SaveSuitabilities(ResourceType);
            }
        }
    }
}
