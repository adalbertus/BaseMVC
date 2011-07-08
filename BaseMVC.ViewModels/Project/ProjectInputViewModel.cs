using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using BaseMVC.ViewModels.Task;

namespace BaseMVC.ViewModels.Project
{
    public class ProjectInputViewModel
    {        
        public int Id { get; set; }

        [Required]
        [Display(Name="Nazwa")]
        public string Name { get; set; }

        [Required]
        [Display(Name="Data rozpoczęcia")]
        public DateTime StartDate { get; set; }

        [Display(Name="Data zakończenia")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Display(Name="Uczestnicy")]
        public IEnumerable<int> SelectedParticipants { get; set; }
        public IEnumerable<ListItem> AvaiableParticipants { get; set; }

        [Required]
        [Display(Name="Właściciel")]
        public int SelectedOwnerId { get; set; }
        public IEnumerable<ListItem> AvaiableOwners { get; set; }

        public IEnumerable<TaskListItemViewModel> Tasks { get; set; }

        public ProjectInputViewModel()
        {
            StartDate = DateTime.Now;
            SelectedParticipants = new List<int>();
        }
    }
}
