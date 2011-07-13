using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NHibernate;
using BaseMVC.Controllers;
using BaseMVC.ViewModels.Project;
using BaseMVC.Tests.IoC;
using NHibernate.Linq;
using BaseMVC.Domain;
using System.Web.Mvc;

namespace BaseMVC.Tests.Controllers
{
    [TestFixture]
    public class WhenProjectManagerInvokeAddingNewProjectFilledWithValidData : ControllerTestBase
    {        
        private ProjectInputViewModel _viewModel;
        private ActionResult AddActionResult { get; set;}

        [TestFixtureSetUp]
        public void SetupContext()
        {
            var projectController = new ProjectController(Session);
            _viewModel = new ProjectInputViewModel
            {
                Name = "Sample project name",
                StartDate = DateTime.Now,
            };
            AddActionResult = projectController.Add(_viewModel);
        }
        
        [Test]
        public void ShouldNewProjectBeWrittenInDatabase()
        {
            var projects = Session.QueryOver<Project>()
                .Where(x => x.Name == _viewModel.Name)
                .And(x => x.StartDate == _viewModel.StartDate)
                .List();

            Assert.That(projects, Has.Count.EqualTo(1));            
        }

        [Test]
        public void ShouldBeRedirectedToProjectListPage()
        {
            var redirectResult = AddActionResult as RedirectToRouteResult;
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("Index"));
        }
    }
}
