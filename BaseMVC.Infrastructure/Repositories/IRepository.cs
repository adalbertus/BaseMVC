using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.Domain;
using BaseMVC.ViewModels;

namespace BaseMVC.Infrastructure.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        DataPage<TEntity> GetDataPage(int pageNumber);        
        TEntity Load(int id);
        TEntity Get(int id);
        void Save(TEntity entity);
        void Update(TEntity entity);
    }
}
