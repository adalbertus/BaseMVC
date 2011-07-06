using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.Domain;
using BaseMVC.ViewModels;

namespace BaseMVC.Infrastructure.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        IEnumerable<Project> GetProjectListItemsForUser(int userId);

        DataPage<ProjectItem> GetPage(int pageNumber, string orderBy, string searchByName);
        ProjectDetails GetDetails(int projectId);
    }
}
