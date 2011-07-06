using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.Domain;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Automapping;

namespace BaseMVC.Infrastructure.Mappings
{
    public class ProjectMappingOverride : IAutoMappingOverride<Project>
    {
        public virtual void Override(AutoMapping<Project> mapping)
        {
            mapping.HasManyToMany(x => x.Participants).Table("UserProject").Cascade.All();
            mapping.HasMany(x => x.Tasks).Cascade.AllDeleteOrphan();
        }
    }
}
