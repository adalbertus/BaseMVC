using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseMVC.ViewModels
{
    public class ProjectContainer
    {
        public ProjectDetails Details { get; set; }
        
        public IEnumerable<UserItem> AvaiableOwners { get; set; }
        public int SelectedOwnerId { get; set; }

        public IEnumerable<ParticipantItem> AvaiableParticipants { get; set; }
        public IEnumerable<int> SelectedParticipants { get; set; }

        public ProjectContainer()
        {
            Details = new ProjectDetails
            {
                StartDate = DateTime.Now,
            };
            AvaiableParticipants = new List<ParticipantItem>();
            SelectedParticipants = new List<int>();
        }
    }
}
