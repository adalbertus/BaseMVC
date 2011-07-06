using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BaseMVC.Controllers;
using Rhino.Mocks;
using BaseMVC.Infrastructure.Repositories;
using System.Web.Mvc;
using BaseMVC.ViewModels;

namespace BaseMVC.Tests.Controllers
{
    [TestFixture]
    public class ProjectControllerIndexAction
    {        
        private DataPage<ProjectItem> _model;

        [TestFixtureSetUp]
        public void SetupContext()
        {
            var projectRepository = MockRepository.GenerateMock<IProjectRepository>();
            var userRepository = MockRepository.GenerateMock<IUserRepository>();

            var items = new List<ProjectItem>
            {
                new ProjectItem()
            };
            DataPage<ProjectItem> dataPage = new DataPage<ProjectItem>(items, 1, 1, 20);
            projectRepository.Stub(p => p.GetPage(1, "Name", string.Empty)).Return(dataPage);
            var projectController = new ProjectController(projectRepository, userRepository);
            _model = (projectController.Search(null, string.Empty, string.Empty) as ViewResult).Model as DataPage<ProjectItem>;
        }

        [Test]
        public void ShouldReturnOneProjectInTheList()
        {
            Assert.That(_model.Items.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ShouldReturnPageNumberEqualToOne()
        {
            Assert.That(_model.PageNumber, Is.EqualTo(1));
        }

        [Test]
        public void ShouldReturnPageSizeEqualTo20()
        {
            Assert.That(_model.PageSize, Is.EqualTo(20));
        }

        [Test]
        public void ShouldReturnTotalItemsCountEqualToOne()
        {
            Assert.That(_model.TotalItemsCount, Is.EqualTo(1));
        }

    }
}
