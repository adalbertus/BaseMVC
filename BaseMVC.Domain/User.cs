using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaseMVC.Domain
{
    public class User : Entity
    {
        public virtual string LoginName { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Project> AssignedProjects { get; set; }

        public User()
        {
            AssignedProjects = new List<Project>();
        }

        public virtual string GetFullName()
        {
            StringBuilder fullName = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(FirstName))
            {
                fullName.Append(FirstName);
            }

            if (!string.IsNullOrWhiteSpace(LastName))
            {
                if (fullName.Length > 0)
                {
                    fullName.Append(" ");
                }
                fullName.Append(LastName);
            }
            return fullName.ToString();
        }
    }
}
