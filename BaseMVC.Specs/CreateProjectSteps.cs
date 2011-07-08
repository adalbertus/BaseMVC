using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using NUnit.Framework;
using BaseMVC.Controllers;
using BaseMVC.Infrastructure.Repositories;
using Rhino.Mocks;
using System.Web.Mvc;
using BaseMVC.Domain;
using BaseMVC.ViewModels;
using BaseMVC.ViewModels.Project;
using Castle.Windsor;
using System.Collections;
using NHibernate;
using BaseMVC.Tests.Controllers;

namespace BaseMVC.Specs
{
    [Binding]
    public class CreateProjectSteps : ControllerTestBase
    {
        private ActionResult _newProjectPage;
        private ProjectController _projectController;
        private ProjectInputViewModel _projectInputViewModel;
        public CreateProjectSteps()
        {
            _projectController = new ProjectController(Session);
        }

        [Given(@"I am not Project Manager")]
        public void GivenIAmNotProjectManager()
        {
            
        }

        [Given(@"I am Project Manager")]
        public void GivenIAmProjectManager()
        {
            
        }

        [Then(@"I will got ""Insufficient priviledges"" message")]
        public void ThenIWillGotInsufficientPriviledgesMessage()
        {
            Assert.Ignore();
        }

        [When(@"I open New Project page")]
        public void WhenIOpenNewProjectPage()
        {
            GivenIOpenedNewProjectPage();
        }

        [Then(@"New Project page will be filled with default values")]
        public void ThenNewProjectPageWillBeFilledWithDefaultValues()
        {
            _projectInputViewModel = ((_newProjectPage as ViewResult).Model as ProjectInputViewModel);
            Assert.That(_projectInputViewModel.StartDate, Is.LessThan(DateTime.Now));
            Assert.That(_projectInputViewModel.AvaiableOwners, Is.Not.Empty);
            Assert.That(_projectInputViewModel.AvaiableParticipants, Is.Not.Empty);
            Assert.That(_projectInputViewModel.EndDate, Is.Null);
            Assert.That(_projectInputViewModel.Name, Is.Null.Or.Empty);
        }

        [Given(@"I filled New Project page as follows")]
        public void GivenIFilledNewProjectPageAsFollows(Table table)
        {
            _projectInputViewModel = ((_newProjectPage as ViewResult).Model as ProjectInputViewModel);

            Func<Table, string, string> findRowValue = (searchTable, fieldName) =>
            {
                return
                    searchTable.Rows.Where(x => x["Field"] == fieldName).Select(x => x["Value"]).FirstOrDefault();
            };

            var projectName      = findRowValue(table, "Name");
            var projectStartDate = findRowValue(table, "StartDate");

            if (!string.IsNullOrWhiteSpace(projectName))
            {
                _projectInputViewModel.Name = projectName;
            }

            DateTime dateTime;
            if (DateTime.TryParse(projectStartDate, out dateTime))
            {
                _projectInputViewModel.StartDate = dateTime;
            }

        }

        [Given(@"I opened New Project page")]
        public void GivenIOpenedNewProjectPage()
        {
            _newProjectPage = _projectController.Add();
        }

        [Then(@"I will be redirected to Project List page")]
        public void ThenIWillBeRedirectedToProjectListPage()
        {
            var redirectResult = (_newProjectPage as RedirectToRouteResult);
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("Search"));
        }

        [When(@"I press Save button")]
        public void WhenIPressSaveButton()
        {
            var projectInput = ((_newProjectPage as ViewResult).Model as ProjectInputViewModel);
            _newProjectPage = _projectController.Add(projectInput);
        }

        [Then(@"project will be saved in database")]
        public void ThenProjectWillBeSavedInDatabase()
        {
            var projects = Session.QueryOver<Project>()
                            .Where(x => x.Name == _projectInputViewModel.Name)
                            .And(x => x.StartDate == _projectInputViewModel.StartDate)
                            .List();

            Assert.That(projects, Has.Count.EqualTo(1));
        }
    }
}
