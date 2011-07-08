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

namespace BaseMVC.NBehave
{

    public class ProjectNB
    {
        public string Name { get; set; }
        public string StartDate { get; set; }
    }

    [ActionSteps]
    public class NewProject
    {
        private ActionResult _newProjectPage;
        private IProjectRepository _projectRepository;
        private ProjectController _projectCtrl;

        public NewProject()
        {
            AutoMapperMock.Configure();

            _projectRepository= MockRepository.GenerateStub<IProjectRepository>();
            var users = new List<User>
            {
                new User { Id = 1, FirstName = "Jan", LastName = "Kowalski" },
            };

            var userPage = new DataPage<User>(users, 1, 1, 1);
            var userRepository = MockRepository.GenerateStub<IUserRepository>();
            //userRepository.Stub(x => x.GetDataPage(1)).Return(userPage);

            _projectCtrl = new ProjectController(MockRepository.GenerateStub<ISession>());
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
            var projectInput = ((_newProjectPage as ViewResult).Model as ProjectInputViewModel);
            Assert.That(projectInput.StartDate, Is.LessThan(DateTime.Now));
            Assert.That(projectInput.AvaiableOwners, Is.Not.Empty);
            Assert.That(projectInput.AvaiableParticipants, Is.Not.Empty);
            Assert.That(projectInput.EndDate, Is.Null);
            Assert.That(projectInput.Name, Is.Null.Or.Empty);
        }

        [Given("I filled New Project page as follows:")]
        public void Given_I_filled_New_Project_page_as_follows(string name, string startDate)
        {
            var projectInput = ((_newProjectPage as ViewResult).Model as ProjectInputViewModel);
            projectInput.Name = name.Trim();
            DateTime dateTime;
            if (DateTime.TryParse(startDate.Trim(), out dateTime))
            {
                projectInput.StartDate = dateTime;
            }
        }

        [When("I press Save button")]
        public void When_I_press_Save_button()
        {
            var projectInput = ((_newProjectPage as ViewResult).Model as ProjectInputViewModel);
            _newProjectPage = _projectCtrl.Add(projectInput);    
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
            _projectRepository.AssertWasCalled(x => x.Save(null), opt => opt.IgnoreArguments());
        }
    }
}
