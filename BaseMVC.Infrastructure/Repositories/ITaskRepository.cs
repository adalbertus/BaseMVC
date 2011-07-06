using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.Domain;
using BaseMVC.ViewModels;

namespace BaseMVC.Infrastructure.Repositories
{
    public interface ITaskRepository : IRepository<Task>
    {
        DataPage<Task> GetDataPage(int pageNumber);
        IEnumerable<Task> GetTasksForProject(int id);
    }
}
