using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBehave.Narrator.Framework;
using System.Web.Mvc;
using BaseMVC.Infrastructure.Repositories;
using BaseMVC.Controllers;
using Rhino.Mocks;
using BaseMVC.Domain;
using BaseMVC.ViewModels;
using BaseMVC.ViewModels.Project;
using NUnit.Framework;
using NHibernate;
using BaseMVC.Tests.Controllers;

namespace BaseMVC.NBehave
{

    public class ProjectNB
    {
        public string Name { get; set; }
        public string StartDate { get; set; }
    }

    [ActionSteps]
    public class NewProject : ControllerTestBase
    {
        private ActionResult _newProjectPage;
        private ProjectInputViewModel _projectInputViewModel;
        private ProjectController _projectCtrl;

        public NewProject()
        {
            _projectCtrl = new ProjectController(Session);
        }

        [Given("I am Project Manager")]
        public void Given_I_am_Project_Manager()
        {
        }

        [When("I open New Project page")]
        [Given("I opened New Project page")]
        public void When_I_open_New_Project_page()
        {
            _newProjectPage = _projectCtrl.Add();
        }

        [Then("New Project page will be filled with default values")]
        public void Then_New_Project_page_will_be_filled_with_default_values()
        {
            _projectInputViewModel = ((_newProjectPage as ViewResult).Model as ProjectInputViewModel);
            Assert.That(_projectInputViewModel.StartDate, Is.LessThan(DateTime.Now));
            Assert.That(_projectInputViewModel.AvaiableOwners, Is.Not.Empty);
            Assert.That(_projectInputViewModel.AvaiableParticipants, Is.Not.Empty);
            Assert.That(_projectInputViewModel.EndDate, Is.Null);
            Assert.That(_projectInputViewModel.Name, Is.Null.Or.Empty);
        }

        [Given("I filled New Project page as follows:")]
        public void Given_I_filled_New_Project_page_as_follows(string name, string startDate)
        {
            _projectInputViewModel = ((_newProjectPage as ViewResult).Model as ProjectInputViewModel);
            _projectInputViewModel.Name = name.Trim();
            DateTime dateTime;
            if (DateTime.TryParse(startDate.Trim(), out dateTime))
            {
                _projectInputViewModel.StartDate = dateTime;
            }
        }

        [When("I press Save button")]
        public void When_I_press_Save_button()
        {
            var projectInputViewModel = ((_newProjectPage as ViewResult).Model as ProjectInputViewModel);
            _newProjectPage = _projectCtrl.Add(projectInputViewModel);    
        }

        [Then("I will be redirected to Project List page")]
        public void Then_I_will_be_redirected_to_Project_List_page()
        {
            var redirectResult = (_newProjectPage as RedirectToRouteResult);
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("Search"));
        }

        [Then("project will be saved in database")]
        public void Then_project_will_be_saved_in_database()
        {
            var projects = Session.QueryOver<Project>()
                            .Where(x => x.Name == _projectInputViewModel.Name)
                            .And(x => x.StartDate == _projectInputViewModel.StartDate)
                            .List();

            Assert.That(projects, Has.Count.EqualTo(1));
        }
    }
}
