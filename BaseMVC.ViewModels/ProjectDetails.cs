using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BaseMVC.ViewModels.Task;

namespace BaseMVC.ViewModels
{
    [Obsolete("Refactor")]
    public class ProjectDetails
    {
        [Required]
        [Display(Name="Nazwa")]
        public string Name { get; set; }

        [Required]
        [Display(Name="Data rozpoczęcia")]
        public DateTime StartDate { get; set; }

        [Display(Name="Data zakończenia")]        
        public DateTime? EndDate { get; set; }

        [Display(Name="Uczestnicy")]
        public IEnumerable<UserItem> Participants { get; set; }

        [Display(Name="Zadania")]
        public IEnumerable<TaskListItem> Tasks { get; set; }

        [Required]
        [Display(Name="Właściciel")]
        public UserItem Owner { get; set; }
    }
}