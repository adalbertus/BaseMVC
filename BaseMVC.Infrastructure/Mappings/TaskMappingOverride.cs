using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.Domain;
using FluentNHibernate.Automapping.Alterations;
using FluentNHibernate.Automapping;

namespace BaseMVC.Infrastructure.Mappings
{
    public class TaskMappingOverride : IAutoMappingOverride<Task>
    {
        public virtual void Override(AutoMapping<Task> mapping)
        {
            mapping.References(x => x.Owner).Column("UserId");            
        }
    }
}
