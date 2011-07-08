using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseMVC.ViewModels
{
    [Obsolete("This DTO will be removed")]
    public class ProjectItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnerFullName { get { return string.Format("{0} {1}", OwnerFirstName, OwnerLastName); } }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalTasksCount { get; set; }
        public int TotalTaskHours { get; set; }
        public int ParticipantsCount { get; set; }

        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
    }
}