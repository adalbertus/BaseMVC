using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseMVC.Domain
{
    public class User : Entity
    {
        public virtual string LoginName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Project> AssignedProjects { get; set; }

        public User()
        {
            AssignedProjects = new List<Project>();
        }
    }
}
