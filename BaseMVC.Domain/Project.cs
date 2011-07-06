using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseMVC.Domain
{
    public class Project : Entity
    {
        public virtual string Name { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual User Owner { get; set; }
        public virtual IList<Task> Tasks { get; private set; }
        public virtual IList<User> Participants { get; private set; }

        public Project()
        {
            StartDate    = DateTime.Now;
            Tasks        = new List<Task>();
            Participants = new List<User>();
        }

        public virtual void AddTask(Task task)
        {
            task.Project = this;
            Tasks.Add(task);
        }

        public static Project CreateProject(string name, DateTime startDate, DateTime? endDate, User owner, IEnumerable<User> participants)
        {
            var project = new Project
            {
                Name         = name,
                StartDate    = startDate,
                EndDate      = endDate,
                Owner        = owner,
            };
            foreach (var participant in participants)
            {
                project.Participants.Add(participant);
            }
            return project;
        }
    }
}
