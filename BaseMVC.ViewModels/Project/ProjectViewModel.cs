using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.ViewModels.Task;
using BaseMVC.ViewModels.User;

namespace BaseMVC.ViewModels.Project
{
    public class ProjectViewModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string OwnerFullName { get; set; }
        public int OwnerId { get; set; }
        public IEnumerable<UserListItemViewModel> Participants { get; set; }
        public IEnumerable<TaskListItemViewModel> Tasks { get; set; }
   }
}
