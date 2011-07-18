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
    public class AvaiableProductsResolver : ValueResolver<int, IEnumerable<ListItem>>
    {
        public ISession Session { get; private set; }

        public AvaiableProductsResolver(ISession session)
        {
            Session = session;
        }

        protected override IEnumerable<ListItem> ResolveCore(int userId)
        {
            using (var tx = Session.BeginTransaction())
            {
                var projects = Session.QueryOver<Project>()
                    .JoinQueryOver<User>(x => x.Participants)
                    .Where(x => x.Id == userId)
                    .List();
                tx.Commit();

                var avaiableProjects = projects.Select(x => new ListItem
                                                {
                                                    Id         = x.Id,
                                                    Value      = x.Name,
                                                    IsSelected = false,
                                                });

                return avaiableProjects;
            }
        }
    }
}