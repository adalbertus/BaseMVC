using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseMVC.ViewModels.Task;
using BaseMVC.ViewModels.Project;
using AutoMapper;
using BaseMVC.Domain;
using BaseMVC.ViewModels;
using NHibernate;

namespace BaseMVC.Tests.AutoMapper
{
    public class AutoMapperMock
    {
        public static void Configure()
        {
            Mapper.CreateMap<Task, TaskInputViewModel>()
                .ForMember(m => m.AvaiableProjects, opt => opt.UseValue(new List<ListItem>()));

            Mapper.CreateMap<TaskInputViewModel, Task>()
                .ForMember(m => m.Owner, o => o.UseValue(new User()))
                .ForMember(m => m.Project, o => o.UseValue(new Project()));

            Mapper.CreateMap<ProjectInputViewModel, Project>()
                .ForMember(m => m.Owner, o => o.UseValue(new User()))
                .ForMember(dst => dst.Participants, o => o.UseValue(new List<User>()))
                .ForMember(dst => dst.Tasks, opt => opt.Ignore());

            Mapper.CreateMap<Task, TaskListItemViewModel>();
            Mapper.CreateMap<Project, ProjectListItemViewModel>();

            Mapper.AssertConfigurationIsValid();
        }
    }

}
