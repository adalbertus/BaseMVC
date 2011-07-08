using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseMVC.ViewModels.Task
{
    public class TaskInputViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int ProjectId { get; set; }
        public int OwnerId { get; set; }

        public IEnumerable<ListItem> AvaiableProjects { get; set; }

        public TaskInputViewModel()
        {
            StartTime = DateTime.Now;
        }
    }
}
