using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.Domain;
using NHibernate;
using BaseMVC.ViewModels;

namespace BaseMVC.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly int _pageSize;
        public int PageSize { get { return _pageSize; } }

        private readonly ISession _session;
        public ISession Session { get { return _session; } }

        public Repository(int pageSize, ISession session)
        {
            _pageSize = pageSize;
            _session  = session;
        }

        #region ITaskRepository Members
        public TEntity Load(int id)
        {
            return Session.Load<TEntity>(id);
        }

        public TEntity Get(int id)
        {
            return Session.Get<TEntity>(id);
        }

        public DataPage<TEntity> GetDataPage(int pageNumber)
        {
            var firstResult = _pageSize * (pageNumber - 1);
            using (var tx = _session.BeginTransaction())
            {
                var page = GetDataPage(_session.QueryOver<TEntity>(), pageNumber);
                tx.Commit();
                return page;
            }
        }

        public void Save(TEntity entity)
        {
            using (var tx = Session.BeginTransaction())
            {
                Session.Save(entity);
                tx.Commit();
            }
        }

        public void Update(TEntity entity)
        {
            using (var tx = Session.BeginTransaction())
            {
                Session.Update(entity);
                tx.Commit();
            }
        }


        protected DataPage<TEntity> GetDataPage(IQueryOver<TEntity> query, int pageNumber)
        {
            var firstResult = _pageSize * (pageNumber - 1);
            var totalCount  = query.ToRowCountQuery().FutureValue<int>();
            var entities    = query.Take(_pageSize).Skip(firstResult).Future();
            var page        = new DataPage<TEntity>(entities, pageNumber, totalCount.Value, _pageSize);
            return page;
        }

        protected DataPage<TResult> GetDataPage<TResult>(IQueryOver<TEntity, TEntity> query, IQueryOver<TEntity, TEntity> totalRowCountQuery, int pageNumber)
        {
            var firstResult = _pageSize * (pageNumber - 1);
            var totalCount  = totalRowCountQuery.FutureValue<int>();
            var entities    = query.Take(_pageSize).Skip(firstResult).Future<TResult>();
            var page        = new DataPage<TResult>(entities, pageNumber, totalCount.Value, _pageSize);
            return page;
        }

        #endregion
    }
}
