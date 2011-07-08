using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseMVC.ViewModels.Project
{
    public class ProjectListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public int TasksCount { get; set; }
        public int ParticipantsCount { get; set; }
    }
}
