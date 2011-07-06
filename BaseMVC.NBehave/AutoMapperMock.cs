﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using BaseMVC.ViewModels.Task;
using BaseMVC.Domain;
using BaseMVC.ViewModels;
using BaseMVC.ViewModels.Project;

namespace BaseMVC.NBehave
{
    public class AutoMapperMock
    {
        public static void Configure()
        {
            Mapper.CreateMap<Task, TaskInput>()
                .ForMember(m => m.AvaiableProjects, opt => opt.UseValue(new List<ListItem>()));

            Mapper.CreateMap<TaskInput, Task>()
                .ForMember(m => m.Owner, o => o.UseValue(new User()))
                .ForMember(m => m.Project, o => o.UseValue(new Project()));

            Mapper.CreateMap<ProjectInput, Project>()
                .ForMember(m => m.Owner, o => o.UseValue(new User()))
                .ForMember(dst => dst.Participants, o => o.UseValue(new List<User>()))
                .ForMember(dst => dst.Tasks, opt => opt.Ignore());

            Mapper.CreateMap<Task, TaskListItem>();
        }

    }
}
