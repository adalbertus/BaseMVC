using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.ViewModels.Task;

namespace BaseMVC.ViewModels.Project
{
    public class ProjectViewModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string OwnerFullName { get; set; }
        public IEnumerable<string> ParticipantsFullNames { get; set; }
        public IEnumerable<TaskListItemViewModel> Tasks { get; set; }
   }
}
