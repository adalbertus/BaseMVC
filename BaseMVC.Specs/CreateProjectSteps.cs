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

namespace BaseMVC.Specs
{
    [Binding]
    public class CreateProjectSteps
    {
        private ActionResult _newProjectPage;
        private IProjectRepository _projectRepository;
        private ProjectController _projectCtrl;
        public CreateProjectSteps()
        {
            AutoMapperMock.Configure();

            _projectRepository= MockRepository.GenerateStub<IProjectRepository>();
            var users = new List<User>
            {
                new User { Id = 1, FirstName = "Jan", LastName = "Kowalski" },
            };

            var userPage = new DataPage<User>(users, 1, 1, 1);
            var userRepository = MockRepository.GenerateStub<IUserRepository>();
            userRepository.Stub(x => x.GetDataPage(1)).Return(userPage);

            _projectCtrl = new ProjectController(_projectRepository, userRepository);
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
            var projectInput = ((_newProjectPage as ViewResult).Model as ProjectInput);
            Assert.That(projectInput.StartDate, Is.LessThan(DateTime.Now));
            Assert.That(projectInput.AvaiableOwners, Is.Not.Empty);
            Assert.That(projectInput.AvaiableParticipants, Is.Not.Empty);
            Assert.That(projectInput.EndDate, Is.Null);
            Assert.That(projectInput.Name, Is.Null.Or.Empty);
        }

        [Given(@"I filled New Project page as follows")]
        public void GivenIFilledNewProjectPageAsFollows(Table table)
        {
            var projectInput = ((_newProjectPage as ViewResult).Model as ProjectInput);

            Func<Table, string, string> findRowValue = (searchTable, fieldName) =>
            {
                return
                    searchTable.Rows.Where(x => x["Field"] == fieldName).Select(x => x["Value"]).FirstOrDefault();
            };

            var projectName      = findRowValue(table, "Name");
            var projectStartDate = findRowValue(table, "StartDate");

            if (!string.IsNullOrWhiteSpace(projectName))
            {
                projectInput.Name = projectName;
            }

            DateTime dateTime;
            if (DateTime.TryParse(projectStartDate, out dateTime))
            {
                projectInput.StartDate = dateTime;
            }

        }

        [Given(@"I opened New Project page")]
        public void GivenIOpenedNewProjectPage()
        {
            _newProjectPage = _projectCtrl.Add();
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
            var projectInput = ((_newProjectPage as ViewResult).Model as ProjectInput);
            _newProjectPage = _projectCtrl.Add(projectInput);
        }

        [Then(@"project will be saved in database")]
        public void ThenProjectWillBeSavedInDatabase()
        {            
            _projectRepository.AssertWasCalled(x => x.Save(null), opt => opt.IgnoreArguments());
        }
    }
}
