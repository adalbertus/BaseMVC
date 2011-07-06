using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.Domain;
using NHibernate;
using NHibernate.Linq;
using BaseMVC.ViewModels;
using NHibernate.Transform;
using NHibernate.Criterion;
using System.Linq.Expressions;
using NHibernate.Criterion.Lambda;
using Castle.Core.Logging;
using BaseMVC.ViewModels.Task;

namespace BaseMVC.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(int pageSize, ISession session)
            : base(pageSize, session)
        {
        }

        #region IProjectRepository Members
        public IEnumerable<Project> GetProjectListItemsForUser(int userId)
        {
            Project projectAlias  = null;
            User participantAlias = null;

            using (var tx = Session.BeginTransaction())
            {
                var projects = Session.QueryOver<Project>(() => projectAlias)
                    .JoinAlias(() => projectAlias.Participants, () => participantAlias)
                    .Where(x => participantAlias.Id == userId)
                    .List();
                tx.Commit();
                return projects;
            }
        }

        public DataPage<ProjectItem> GetPage(int pageNumber, string orderBy, string searchByName)
        {
            Project projectAlias    = null;
            User participantAlias   = null;
            User ownerAlias         = null;
            Task taskAlias          = null;
            ProjectItem projectItem = null;
            var firstResult         = PageSize * (pageNumber - 1);

            using (var tx = Session.BeginTransaction())
            {
                var projectsJoinQuery = Session.QueryOver<Project>(() => projectAlias)
                    .JoinAlias(() => projectAlias.Owner, () => ownerAlias)
                    .Left.JoinAlias(() => projectAlias.Participants, () => participantAlias)
                    .Left.JoinAlias(() => projectAlias.Tasks, () => taskAlias);
                if (!string.IsNullOrEmpty(searchByName))
                {
                    projectsJoinQuery = projectsJoinQuery.Where(p => p.Name.IsLike(searchByName, MatchMode.Start));
                }

                var totalCountQuery = projectsJoinQuery.Clone()
                    .SelectList(l => l.SelectCountDistinct(p => p.Id));

                var projectsListQuery = projectsJoinQuery
                    .SelectList(l => l
                        .SelectGroup(p => p.Id).WithAlias(() => projectItem.Id)
                        .SelectGroup(p => p.Name).WithAlias(() => projectItem.Name)
                        .SelectGroup(p => ownerAlias.FirstName).WithAlias(() => projectItem.OwnerFirstName)
                        .SelectGroup(p => ownerAlias.LastName).WithAlias(() => projectItem.OwnerLastName)
                        .SelectGroup(p => p.StartDate).WithAlias(() => projectItem.StartDate)
                        .SelectGroup(p => p.EndDate).WithAlias(() => projectItem.EndDate)
                        .SelectCountDistinct(p => participantAlias.Id).WithAlias(() => projectItem.ParticipantsCount)
                        .SelectCountDistinct(p => taskAlias.Id).WithAlias(() => projectItem.TotalTasksCount)
                        )
                    .TransformUsing(Transformers.AliasToBean<ProjectItem>());;

                switch (orderBy)
                {
                    case "Name":
                        projectsListQuery = projectsListQuery.OrderByAlias(() => projectItem.Name).Asc;
                        break;
                    case "Owner":
                        projectsListQuery = projectsListQuery
                            .OrderByAlias(() => projectItem.OwnerLastName).Asc
                            .OrderByAlias(() => projectItem.OwnerFirstName).Asc;                        
                        break;
                    case "StartDate":
                        projectsListQuery = projectsListQuery
                            .OrderByAlias(() => projectItem.StartDate).Asc;                            
                        break;
                }
                
                //projectsListQuery = projectsListQuery.TransformUsing(Transformers.AliasToBean<ProjectItem>());

                var page = GetDataPage<ProjectItem>(projectsListQuery, totalCountQuery, pageNumber);
                
                foreach (var item in page.Items)
                {
                    var tasks = Session.QueryOver<Task>()
                        .Where(t => t.Project.Id == item.Id)
                        .List();
                    item.TotalTaskHours = tasks.Sum(t => t.GetTotalSpendHours());
                    
                }

                tx.Commit();
                
                return page;
            }
            /*
            // inne podejście bez joinów (5 zapytan - a właściwie: ilość produktów * 2 + 1)
            var tmpProjList = Session.QueryOver<Project>().Future();
            var altProjList = new List<ProjectItem>();
            foreach (var proj in tmpProjList)
            {
                var p = new ProjectItem
                {
                    Id                = proj.Id,
                    Name              = proj.Name,
                    StartDate         = proj.StartDate,
                    EndDate           = proj.EndDate,
                    ParticipantsCount = proj.Participants.Count,
                    TotalTasksCount   = proj.Tasks.Count
                };

                altProjList.Add(p);
            }
            */
        }

        public ProjectDetails GetDetails(int projectId)
        {
            var projectDetails = new ProjectDetails();
            using (var tx  = Session.BeginTransaction())
            {
                var projects = Session.QueryOver<Project>()
                        .Where(p => p.Id == projectId).Future();

                TaskListItem taskListItem = null;
                var tasks = Session.QueryOver<Task>()
                        .Where(t => t.Project.Id == projectId)
                        .SelectList(l => l
                            .Select(t => t.Id).WithAlias(() => taskListItem.Id)
                            .Select(t => t.Title).WithAlias(() => taskListItem.Title)
                        )
                        .TransformUsing(Transformers.AliasToBean<TaskListItem>())
                        .Future<TaskListItem>();

                User participantAlias = null;
                UserItem participantListItem = null;

                var participants = Session.QueryOver<Project>()
                        .Where(p => p.Id == projectId)
                        .JoinQueryOver(p => p.Participants, () => participantAlias)
                        .SelectList(l => l
                            .Select(p => participantAlias.Id).WithAlias(() => participantListItem.Id)
                            .Select(p => participantAlias.FirstName).WithAlias(() => participantListItem.FirstName)
                            .Select(p => participantAlias.LastName).WithAlias(() => participantListItem.LastName)
                        )
                        .TransformUsing(Transformers.AliasToBean<UserItem>())
                        .Future<UserItem>();
               
                var project                 = projects.FirstOrDefault();
                if (project == null)
                {
                    return null;
                }
                projectDetails.Name         = project.Name;
                projectDetails.StartDate    = project.StartDate;
                projectDetails.Tasks        = tasks;
                projectDetails.Participants = participants;
                if (project.Owner != null)
                {
                    projectDetails.Owner    = new UserItem
                    {
                        Id        = project.Owner.Id,
                        FirstName = project.Owner.FirstName,
                        LastName  = project.Owner.LastName,
                    };
                }

                tx.Commit();
            }
            return projectDetails;
        }
        #endregion
    }
}
