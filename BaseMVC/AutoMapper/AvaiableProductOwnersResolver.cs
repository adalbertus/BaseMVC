using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BaseMVC.ViewModels;
using BaseMVC.Infrastructure.Repositories;
using NHibernate;
using BaseMVC.Domain;

namespace BaseMVC.AutoMapper
{
    public class AvaiableProductOwnersResolver : ValueResolver<Project, IEnumerable<ListItem>>
    {
        public ISession Session { get; private set; }

        public AvaiableProductOwnersResolver(ISession session)
        {
            Session = session;
        }

        protected override IEnumerable<ListItem> ResolveCore(Project project)
        {
            var users          = Session.QueryOver<User>().List();
            var avaiableOwners = users.Select(x => new ListItem
                                                {
                                                    Id         = x.Id,
                                                    Value      = x.GetFullName(),
                                                    IsSelected = false,
                                                });

            return avaiableOwners;
        }
    }
}