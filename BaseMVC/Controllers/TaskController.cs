using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate;
using BaseMVC.Infrastructure.Repositories;
using BaseMVC.ViewModels.Task;
using BaseMVC.ViewModels;
using BaseMVC.Domain;
using BaseMVC.AutoMapper;
using AutoMapper;

namespace BaseMVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        
        public TaskController(IProjectRepository projectRepository, ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _projectRepository = projectRepository;
            _taskRepository    = taskRepository;
            _userRepository    = userRepository;
        }

        [HttpGet]
        public ActionResult ShowForProject(int id)
        {
            var projects = _taskRepository.GetTasksForProject(id);
            var taskList = projects.Map<TaskListItem[]>();

            return View(taskList);
        }

        [HttpGet]
        public ActionResult Add(int? projectId = null)
        {
            var task = CreateUpdateTaskInput();
            if (projectId.HasValue)
            {
                task.ProjectId = projectId.Value;
            }
            return View(task);
        }

        [HttpPost]
        public ActionResult Add(TaskInput task)
        {
            task = CreateUpdateTaskInput(task);
            if (ModelState.IsValid)
            {
                var project = _projectRepository.Load(task.ProjectId);

                var taskModel = task.Map<Task>();

                project.AddTask(taskModel);
                _projectRepository.Save(project);
                return RedirectToAction("Index", "Home");
            }
            return View(task);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var taskModel = _taskRepository.Get(id);
            var taskInput = taskModel.Map<TaskInput>();
            return View(taskInput);
        }

        [HttpPost]
        public ActionResult Edit(TaskInput task)
        {            
            if (ModelState.IsValid)
            {
                var taskModel = task.Map<Task>();

                _taskRepository.Update(taskModel);
                return RedirectToAction("Index", "Home");
            }

            var avaiableProjects = GetAvaiableProjectsForCurrentUser();
            task.AvaiableProjects = avaiableProjects;
            return View(task);
        }


        private TaskInput CreateUpdateTaskInput(TaskInput task = null)
        {
            int userId = 1;
            var avaiableProjects = GetAvaiableProjectsForCurrentUser();

            if (task == null)
            {
                task = new TaskInput
                            {
                                StartTime        = DateTime.Now,
                                AvaiableProjects = avaiableProjects,
                                OwnerId          = userId,
                            };
            }
            else
            {
                task.AvaiableProjects = avaiableProjects;
            }

            return task;
        }

        private IEnumerable<ListItem> GetAvaiableProjectsForCurrentUser()
        {
            int userId = 1;
            var avaiableProjects = _projectRepository.GetProjectListItemsForUser(userId)
                .Select(x => new ListItem
                {
                    Id         = x.Id,
                    Value      = x.Name,
                    IsSelected = false,
                });

            return avaiableProjects;
        }
    }
}
