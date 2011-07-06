using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.Domain;
using NHibernate;
using BaseMVC.ViewModels;

namespace BaseMVC.Infrastructure.Repositories
{
    public class TaskRepository : Repository<Task>,  ITaskRepository
    {
        public TaskRepository(int pageSize, ISession session)
            : base(pageSize, session)
        {
        }

        #region ITaskRepository Members


        public IEnumerable<Task> GetTasksForProject(int id)
        {
            return Session.QueryOver<Task>()
                .Where(x => x.Project.Id == id)
                .List();
        }

        #endregion
    }
}
