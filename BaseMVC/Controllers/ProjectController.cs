using System.Linq;
using System.Web.Mvc;
using BaseMVC.AutoMapper;
using BaseMVC.Domain;
using BaseMVC.Infrastructure.Repositories;
using BaseMVC.ViewModels;
using BaseMVC.ViewModels.Project;

namespace BaseMVC.Controllers
{
    public class ProjectController : BaseMVCController
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public ProjectController(IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _userRepository    = userRepository;
        }

        [HttpGet]
        public ActionResult Search(int? pageNumber, string orderBy, string name)
        {
            // simulate slow repository connection
            //System.Threading.Thread.Sleep(2 * 1000);
            var projectListPage = _projectRepository.GetPage(pageNumber.GetValueOrDefault(1), orderBy, name);

            if (IsAjaxRequest)
            {
                return PartialView("_ProjectList", projectListPage);
            }
            else
            {
                return View(projectListPage);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var projectDetails = _projectRepository.GetDetails(id);
            if (projectDetails == null)
            {
                return HttpNotFound("Project not found");
            }
            return View(projectDetails);
        }
        
        [HttpGet]
        public ActionResult Add()
        {
            var newProject = CreateUpdateProjectInput();
            return View(newProject);
        }

        [HttpPost]
        public ActionResult Add(ProjectInput newProject)
        {
            newProject = CreateUpdateProjectInput(newProject);
            
            if (ModelState.IsValid)
            {
                var projectModel = newProject.Map<Project>();
                _projectRepository.Save(projectModel);
                return RedirectToAction("Search");
            }

            return View(newProject);
        }

        private ProjectInput CreateUpdateProjectInput(ProjectInput newProject = null)
        {
            if (newProject == null)
            {
                newProject = new ProjectInput();
            }

            var avaiableOwners              = _userRepository.GetDataPage(1).Items;
            var avaiableParticipants        = _userRepository.GetDataPage(1).Items;

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
