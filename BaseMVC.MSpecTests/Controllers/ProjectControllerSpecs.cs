using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using BaseMVC.Controllers;
using NHibernate;
using NHibernate.Linq;
using Rhino.Mocks;
using BaseMVC.ViewModels.Project;
using System.Web.Mvc;
using BaseMVC.Domain;

namespace BaseMVC.MSpecTests.Controllers
{
    [Subject("As ProjectManager")]
    public class when_invoking_add_method_with_valid_input_form : ControllerSpecsBase<ProjectController>
    {
        private static ProjectInputViewModel viewModel;
        private static ActionResult actionResult;       
        
        Establish context =() =>
            {                
                viewModel = new ProjectInputViewModel
                {
                    Name = "Sample project name",
                    StartDate = DateTime.Now,
                };                
            };

        Because controller_invoke_add_method =()=> actionResult = controller.Add(viewModel);

        It should_save_given_input_to_database =() => session
            .QueryOver<Project>()
            .Where(x => x.Name == viewModel.Name)
            .And(x => x.StartDate == viewModel.StartDate)
            .SingleOrDefault()
            .ShouldNotBeNull();
        It should_redirect_to_action_Search =() => (actionResult as RedirectToRouteResult).RouteValues["action"].ShouldBeTheSameAs("Search");
    }

    [Subject("As ProjectManager")]
    public class when_invoking_add_method_with_invalid_model_state : ControllerSpecsBase<ProjectController>
    {
        private static ProjectInputViewModel viewModel;
        private static ActionResult viewResult;

        Establish context =() =>
        {
            viewModel = new ProjectInputViewModel
            {
                Name = "Sample project name",
                StartDate = DateTime.Now,
            };

            controller.ModelState.AddModelError("key", "errorMessage");            
        };

        Because controller_invoke_add_method =() => viewResult = controller.Add(viewModel);

        It should_not_save_given_input_to_database =() => session
            .QueryOver<Project>()
            .Where(x => x.Name == viewModel.Name)
            .And(x => x.StartDate == viewModel.StartDate)
            .SingleOrDefault()
            .ShouldBeNull();
        It should_show_add_form_again =() => viewResult.ShouldBeOfType<ViewResult>();
    }

    [Subject("As ProjectManager")]
    public class when_invoking_add_method_without_parameters : ControllerSpecsBase<ProjectController>
    {
        private static ViewResult viewResult;
        private static string currentDate;

        Establish context =() =>
        {
            currentDate = DateTime.Now.ToString("yyyyMMdd");
        };

        Because controller_invoke_add_method =() => viewResult = controller.Add() as ViewResult;

        It should_return_model_with_StartDate_equal_to_current_date =() => (viewResult.Model as ProjectInputViewModel).StartDate.ToString("yyyyMMdd").ShouldEqual(currentDate);
        It should_return_model_with_not_empty_AvaiableOwners_list =() => (viewResult.Model as ProjectInputViewModel).AvaiableOwners.ShouldNotBeEmpty();
        It should_return_model_with_not_empty_AvaiableParticipants_list =() => (viewResult.Model as ProjectInputViewModel).AvaiableParticipants.ShouldNotBeEmpty();
    }

    [Subject("As ProjectManager")]
    public class when_invoking_details_for_existing_project : ControllerSpecsBase<ProjectController>
    {
        private static ViewResult viewResult;
        private static DateTime startDate = DateTime.Now;
        private static User user;
        private static Project project;

        Establish context =() =>
        {
            user = new User 
                    {
                        FirstName = "FirstName",
                        LoginName = "LoginName",
                    };
            project = Project.CreateProject("Sample project", startDate, null, user, new[] { user });
            session.Save(project);
            session.Flush();
        };

        Because controller_invoke_add_method =() => viewResult = controller.Details(project.Id) as ViewResult;

        It should_return_non_empty_model =() => viewResult.Model.ShouldNotBeNull();
        It should_return_model_with_ProjectName_equal_to_Sample_project = () => (viewResult.Model as ProjectViewModel).Name.ShouldEqual("Sample project");
    }
}
