using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BaseMVC.ViewModels;
using BaseMVC.Infrastructure.Repositories;

namespace BaseMVC.AutoMapper
{
    public class AvaiableProductsResolver : ValueResolver<int, IEnumerable<ListItem>>
    {
        public IProjectRepository ProjectRepository { get; set; }

        public AvaiableProductsResolver(IProjectRepository projectRepository)
        {
            ProjectRepository = projectRepository;
        }

        protected override IEnumerable<ListItem> ResolveCore(int userId)
        {
            var avaiableProjects = ProjectRepository.GetProjectListItemsForUser(userId)
                .Select(x => new ListItem
                {
                    Id         = x.Id,
                    Value      = x.Name,
                    IsSelected = false,
                });

            return avaiableProjects;
        }
    }
}