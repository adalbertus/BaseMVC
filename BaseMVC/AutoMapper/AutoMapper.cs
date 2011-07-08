using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BaseMVC.Domain;
using BaseMVC.ViewModels.Task;
using BaseMVC.ViewModels.Project;
using Castle.Windsor;

namespace BaseMVC.AutoMapper
{
    public static class AutoMapper
    {
        public static void Configure(IWindsorContainer container)
        {
            Mapper.Initialize(x => x.ConstructServicesUsing(type => ResolveType(container, type)));
            
            //Mapper.CreateMap<DateTime, DateTime>().ConvertUsing<UtcToLocalTimeConverter>();
            
            Mapper.CreateMap<Task, TaskInputViewModel>()
                .ForMember(m => m.AvaiableProjects, opt => opt.ResolveUsing<AvaiableProductsResolver>().FromMember(x => x.Owner.Id));

            Mapper.CreateMap<TaskInputViewModel, Task>()
                .ForMember(m => m.Owner, o => o.ResolveUsing<LoadingEntityResolver<User>>().FromMember(x => x.OwnerId))
                .ForMember(m => m.Project, o => o.ResolveUsing<LoadingEntityResolver<Project>>().FromMember(x => x.ProjectId));

            Mapper.CreateMap<ProjectInputViewModel, Project>()
                .ForMember(m => m.Owner, o => o.ResolveUsing<LoadingEntityResolver<User>>().FromMember(x => x.SelectedOwnerId))
                .ForMember(dst => dst.Participants, opt => opt.ResolveUsing<LoadingCollectionEntityResolver<User>>().FromMember(x => x.SelectedParticipants))
                .ForMember(dst => dst.Tasks, opt => opt.Ignore());                
              
            Mapper.CreateMap<Task, TaskListItemViewModel>();
            Mapper.CreateMap<Project, ProjectListItemViewModel>();
        }

        private static object ResolveType(IWindsorContainer container, Type type)
        {
            var resolvedType = container.Resolve(type);
            return resolvedType;
        }
    }
}