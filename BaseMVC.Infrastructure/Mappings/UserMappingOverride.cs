using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.Domain;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Automapping;

namespace BaseMVC.Infrastructure.Mappings
{
    public class UserMappingOverride : IAutoMappingOverride<User>
    {
        public virtual void Override(AutoMapping<User> mapping)
        {
            mapping.HasManyToMany(x => x.AssignedProjects).Table("UserProject").Inverse().Cascade.All();
        }
    }
}
