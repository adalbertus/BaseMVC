using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BaseMVC.Domain;
using NHibernate;

namespace BaseMVC.AutoMapper
{
    public class LoadingEntityResolver<TEntity> : ValueResolver<int, TEntity> where TEntity : Entity
    {
        private readonly ISession _session;

        public LoadingEntityResolver(ISession session)
        {
            _session = session;
        }

        protected override TEntity ResolveCore(int source)
        {
            return _session.Load<TEntity>(source);
        }
    }
}