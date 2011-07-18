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
using BaseMVC.AutoMapper;
using MvcContrib.TestHelper;

namespace BaseMVC.MSpecTests.Controllers
{
    [Subject("As ProjectManager")]
    public class when_invoking_add_method_with_valid_input_form : ControllerSpecsBase<ProjectController>
    {
        private static ProjectInputViewModel viewModel;

        Establish context =() =>
            {                
                viewModel = new ProjectInputViewModel
                {
                    Name = "Sample project name",
                    StartDate = DateTime.Now,
                };                
            };

        Because controller_invoke_add_method =()=> _actionResult = _controller.Add(viewModel);

        It should_save_given_input_to_database =() => _session.QueryOver<Project>()
            .Where(x => x.Name == viewModel.Name)
            .And(x => x.StartDate == viewModel.StartDate)
            .SingleOrDefault()
            .ShouldNotBeNull();
        It should_redirect_to_action_Index =() => _actionResult.AssertActionRedirect().ToAction("Index");
    }

    [Subject("As ProjectManager")]
    public class when_invoking_add_method_with_invalid_model_state : ControllerSpecsBase<ProjectController>
    {
        private static ProjectInputViewModel viewModel;

        Establish context =() =>
        {
            viewModel = new ProjectInputViewModel
            {
                Name = "Sample project name",
                StartDate = DateTime.Now,
            };

            _controller.ModelState.AddModelError("key", "errorMessage");
        };

        Because controller_invoke_add_method =() => _actionResult = _controller.Add(viewModel);

        It should_not_save_given_input_to_database =() => _session
            .QueryOver<Project>()
            .Where(x => x.Name == viewModel.Name)
            .And(x => x.StartDate == viewModel.StartDate)
            .SingleOrDefault()
            .ShouldBeNull();
        It should_show_add_form_again =() => _actionResult.ShouldBeOfType<ViewResult>();
    }

    [Subject("As ProjectManager")]
    public class when_invoking_add_method_without_parameters : ControllerSpecsBase<ProjectController>
    {
        private static string currentDate;

        Establish context =() =>
        {
            currentDate = DateTime.Now.ToString("yyyyMMdd");
        };

        Because controller_invoke_add_method =() => _actionResult = _controller.Add();

        It should_return_model_with_StartDate_equal_to_current_date =() => (ViewResult.Model as ProjectInputViewModel).StartDate.ToString("yyyyMMdd").ShouldEqual(currentDate);
        It should_return_model_with_not_empty_AvaiableOwners_list =() => (ViewResult.Model as ProjectInputViewModel).AvaiableOwners.ShouldNotBeEmpty();
        It should_return_model_with_not_empty_AvaiableParticipants_list =() => (ViewResult.Model as ProjectInputViewModel).AvaiableParticipants.ShouldNotBeEmpty();
    }

    [Subject("As ProjectManager")]
    public class when_invoking_details_for_existing_project : ControllerSpecsBase<ProjectController>
    {
        protected static User johnSmith;
        protected static Project sampleProject;
        protected static ProjectViewModel ProjectViewModel { get { return ViewResult.Model as ProjectViewModel; } }

        Establish context =() =>
        {
            johnSmith    = _databaseCreator.JohnSmith;
            sampleProject = _databaseCreator.SampleProject;
        };

        Because controller_invoke_details_method =() => _actionResult = _controller.Details(sampleProject.Id) as ViewResult;

        It should_return_non_empty_model =() => ViewResult.Model.ShouldNotBeNull();
        It should_return_model_with_Name_equal_to_Sample_project = () => ProjectViewModel.Name.ShouldEqual(sampleProject.Name);
        It should_return_model_with_StartDate_properly_set = () => ProjectViewModel.StartDate.ShouldEqual(sampleProject.StartDate);
        It should_return_model_with_null_EndDate = () => ProjectViewModel.EndDate.ShouldBeNull();
        It should_return_model_with_John_Smith_as_a_owner = () => ProjectViewModel.OwnerFullName.ShouldEqual(johnSmith.GetFullName());
        It should_return_model_with_one_participant = () => ProjectViewModel.Participants.Count().ShouldEqual(1);
        It should_return_model_with_John_Smith_as_a_participant = () => ProjectViewModel.Participants.ShouldContain(x => x.FullName == johnSmith.GetFullName());
    }

    [Subject("As ProjectManager")]
    public class when_invoking_details_for_non_existing_project : ControllerSpecsBase<ProjectController>
    {
        Establish context =() =>
        {
            _databaseCreator.ClearProjectsFromDatabase();
        };

        Because controller_invoke_details_method =() => _actionResult = _controller.Details(1);

        Behaves_like<NonExistingProjectBehavior> non_existing_project;
    }

    [Subject("As ProjectManager")]
    public class when_invoking_edit_for_existing_project : ControllerSpecsBase<ProjectController>
    {        
        protected static ProjectInputViewModel ProjectInputViewModel { get { return ViewResult.Model as ProjectInputViewModel; } }
        protected static User johnSmith;
        protected static Project sampleProject;

        Establish context =() =>
        {
            johnSmith    = _databaseCreator.JohnSmith;
            sampleProject = _databaseCreator.SampleProject;
        };

        Because controller_invoke_edit_method =() => _actionResult = _controller.Edit(sampleProject.Id);

        It should_return_model_with_StartDate_properly_set =() => ProjectInputViewModel.StartDate.ShouldEqual(sampleProject.StartDate);
        It should_return_model_with_project_name_equal_to_Sample_project = () => ProjectInputViewModel.Name.ShouldEqual(sampleProject.Name);
        It should_return_model_with_not_empty_AvaiableOwners_list =() => ProjectInputViewModel.AvaiableOwners.ShouldNotBeEmpty();
        It should_return_model_with_not_empty_AvaiableParticipants_list =() => ProjectInputViewModel.AvaiableParticipants.ShouldNotBeEmpty();
        It should_return_model_with_selected_owner_as_John_Smith = () => ProjectInputViewModel.SelectedOwnerId.ShouldEqual(johnSmith.Id);
    }

    [Subject("As ProjectManager")]
    public class when_invoking_edit_for_non_existing_project : ControllerSpecsBase<ProjectController>
    {
        Establish context =() =>
        {
            _databaseCreator.ClearProjectsFromDatabase();
        };

        Because controller_invoke_edit_method =() => _actionResult = _controller.Edit(1);

        Behaves_like<NonExistingProjectBehavior> non_existing_project;
    }

    [Subject("As ProjectManager")]
    public class when_invoked_edit_with_valid_input_form : ControllerSpecsBase<ProjectController>
    {
        protected static User johnSmith;
        protected static User markTwain;
        protected static Project sampleProject;
        protected static ProjectInputViewModel projectInputViewModel;

        Establish context =() =>
        {
            johnSmith    = _databaseCreator.JohnSmith;
            markTwain = _databaseCreator.MarkTwain;
            sampleProject = _databaseCreator.SampleProject;

            projectInputViewModel = sampleProject.Map<ProjectInputViewModel>();
            projectInputViewModel.Name = "Changed name";
            projectInputViewModel.SelectedOwnerId = markTwain.Id;
            projectInputViewModel.SelectedParticipants = new[] { johnSmith.Id, markTwain.Id };
            _session.Evict(sampleProject);
        };

        Because controller_invoke_edit_method =() => _actionResult = _controller.Edit(projectInputViewModel);

        It should_redirect_to_action_Index =() => _actionResult.AssertActionRedirect().ToAction("Index");
        It should_not_add_nor_remove_project_from_database = () => _session.QueryOver<Project>().RowCount().ShouldEqual(1);
        It should_project_name_be_changed_from_Sample_project_to_Change_name = () => _session.QueryOver<Project>().SingleOrDefault().Name.ShouldEqual("Changed name");
        It should_change_owner_to_Mark_Twain = () => _session.QueryOver<Project>().SingleOrDefault().Owner.Id.ShouldEqual(markTwain.Id);
        It should_have_only_John_Smith_and_Mark_Twain_as_participants = () => _session.QueryOver<Project>().SingleOrDefault().Participants.Select(x => x.Id).ShouldContainOnly(johnSmith.Id, markTwain.Id);
    }

    [Subject("As ProjectManager")]
    public class when_invoked_edit_with_invalid_input_form : ControllerSpecsBase<ProjectController>
    {
        protected static User johnSmith;
        protected static User markTwain;
        protected static Project sampleProject;
        protected static ProjectInputViewModel projectInputViewModel;

        Establish context =() =>
        {
            johnSmith    = _databaseCreator.JohnSmith;
            markTwain = _databaseCreator.MarkTwain;
            sampleProject = _databaseCreator.SampleProject;

            projectInputViewModel = sampleProject.Map<ProjectInputViewModel>();
            projectInputViewModel.Name = string.Empty;
            _session.Evict(sampleProject);
            _controller.ModelState.AddModelError("key", "errorMessage");
        };

        Because controller_invoke_edit_method =() => _actionResult = _controller.Edit(projectInputViewModel);

        It should_show_edit_form_again =() => _actionResult.ShouldBeOfType<ViewResult>();
        It should_not_save_given_input_to_database =() => _session.QueryOver<Project>().Where(x => x.Name == projectInputViewModel.Name).RowCount().ShouldEqual(0);
    }

    [Subject("As ProjectManager")]
    public class when_invoked_delete_for_existing_project : ControllerSpecsBase<ProjectController>
    {
        protected static Project sampleProject;

        Establish context =() =>
        {
            sampleProject = _databaseCreator.SampleProject;
            _session.Evict(sampleProject);
        };

        Because controller_invoke_delete_method =() => _actionResult = _controller.Delete(sampleProject.Id);

        It should_redirect_to_action_Index =() => _actionResult.AssertActionRedirect().ToAction("Index");
        It should_project_be_deleted_from_database =() => _session.Get<Project>(sampleProject.Id).ShouldBeNull();
    }

    [Subject("As ProjectManager")]
    public class when_invoked_delete_for_non_existing_project : ControllerSpecsBase<ProjectController>
    {
        Establish context =() =>
        {
            _databaseCreator.ClearProjectsFromDatabase();
        };

        Because controller_invoke_delete_method =() => _actionResult = _controller.Delete(1);

        Behaves_like<NonExistingProjectBehavior> non_existing_project;
    }

    [Behaviors]
    public class NonExistingProjectBehavior
    {
        protected static ActionResult _actionResult;

        It should_return_page_not_found_result =() => _actionResult.ShouldBeOfType<HttpNotFoundResult>();
        It should_return_page_not_found_with_status_description_Project_not_found =() => (_actionResult as HttpNotFoundResult).StatusDescription.ShouldEqual("Project not found");
    }
}
