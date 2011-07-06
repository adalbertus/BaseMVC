using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BaseMVC.Domain;
using NHibernate;

namespace BaseMVC.AutoMapper
{
    public class LoadingCollectionEntityResolver<TEntity> : ValueResolver<IEnumerable<int>, IEnumerable<TEntity>> where TEntity : Entity
    {
        private readonly ISession _session;

        public LoadingCollectionEntityResolver(ISession session)
        {            
            _session = session;
        }

        protected override IEnumerable<TEntity> ResolveCore(IEnumerable<int> source)
        {
            foreach (var id in source)
            {
                var entity = _session.Load<TEntity>(id);
                yield return entity;
            }
        }
    }
}