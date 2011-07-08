using System.Linq;
using System.Web.Mvc;
using BaseMVC.AutoMapper;
using BaseMVC.Domain;
using BaseMVC.Infrastructure.Repositories;
using BaseMVC.Infrastructure.Extensions;
using BaseMVC.ViewModels;
using BaseMVC.ViewModels.Project;
using NHibernate.Linq;
using System.Collections.Generic;
using System;
using NHibernate;

namespace BaseMVC.Controllers
{
    public class ProjectController : BaseMVCController
    {
        public ProjectController(ISession session)
            : base(session)
        {
        }

        //private readonly IProjectRepository _projectRepository;
        //private readonly IUserRepository _userRepository;

        //public ProjectController(IProjectRepository projectRepository, IUserRepository userRepository)
        //{
        //    _projectRepository = projectRepository;
        //    _userRepository    = userRepository;
        //}

        [Authorize]
        [HttpGet]
        public ActionResult Search(int? pageNumber, string orderBy, string name)
        {
            // simulate slow repository connection
            //System.Threading.Thread.Sleep(2 * 1000);

            // single query version - but we cannot use AutoMapper functionality            
            var projects = Session.Query<Project>()
                .Where(x => x.Owner.LoginName == this.User.Identity.Name)
                .Select(x => new ProjectListItemViewModel
                {
                    Id                = x.Id,
                    Name              = x.Name,
                    TasksCount        = x.Tasks.Count,
                    ParticipantsCount = x.Participants.Count,
                    StartDate         = x.StartDate,
                    EndDate           = x.EndDate,
                    OwnerFirstName    = x.Owner.FirstName,
                    OwnerLastName     = x.Owner.LastName,
                });

            // AutoMapper version causes a lot of queries 1 + #Tasks + #Participants
            //var projectsModel = Session.Query<Project>()
            //    .Where(x => x.Owner.LoginName == this.User.Identity.Name)
            //    .ToFuture();
            //var projectsDisplay = projectsModel.Map<IEnumerable<ProjectListItemViewModel>>();

            // Third solution is to use QueryOver with JoinAlias - but a lot of manual mapping
            //var projectListPage = _projectRepository.GetPage(pageNumber.GetValueOrDefault(1), orderBy, name);

            if (IsAjaxRequest)
            {
                return PartialView("_ProjectList", projects);
            }
            else
            {
                return View(projects);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var project = Session.Get<Project>(id);
            if (project == null)
            {
                return HttpNotFound("Project not found");
            }
            return View(project);
        }
        
        [HttpGet]
        public ActionResult Add()
        {
            var newProject = CreateUpdateProjectInput();
            return View(newProject);
        }

        [HttpPost]
        public ActionResult Add(ProjectInputViewModel newProject)
        {
            newProject = CreateUpdateProjectInput(newProject);
            
            if (ModelState.IsValid)
            {
                using (var tx = Session.BeginTransaction())
                {
                    var projectModel = newProject.Map<Project>();
                    Session.Save(projectModel);
                    tx.Commit();
                }
                return RedirectToAction("Search");
            }

            return View(newProject);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Edit(ProjectInputViewModel projectInput)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            throw new NotImplementedException();
        }

        private ProjectInputViewModel CreateUpdateProjectInput(ProjectInputViewModel newProject = null)
        {
            if (newProject == null)
            {
                newProject = new ProjectInputViewModel();
            }

            var avaiableOwners = Session.Query<User>().ToList();
            var avaiableParticipants = Session.Query<User>().ToList();

            newProject.AvaiableOwners = avaiableOwners.Select(x => new ListItem
            {
                Id         = x.Id,
                IsSelected = false,
                Value      = string.Format("{0} {1}", x.FirstName, x.LastName),
            });

            newProject.AvaiableParticipants = avaiableParticipants.Select(x => new ListItem
            {
                Id         = x.Id,
                IsSelected = newProject.SelectedParticipants.Any(y => y == x.Id),
                Value      = string.Format("{0} {1}", x.FirstName, x.LastName),
            });
            return newProject;
        }
    }
}
