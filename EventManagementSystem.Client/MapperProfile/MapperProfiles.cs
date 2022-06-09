using AutoMapper;
using EventManagementSystem.ViewModels;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagementSystem.MapperProfile
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<DateTime, Timestamp>()
                .ReverseMap();
            CreateMap<EmployeeModel, EmployeeViewModel>()
                    .ReverseMap();
            CreateMap<EventModel, EventViewModel>()
                    .ReverseMap();
            CreateMap<EventCommentsModel, EventCommentViewModel>()
                    .ReverseMap();
            CreateMap<EventImageModel, EventImagesViewModel>()
                    .ReverseMap();
        }
    }
}
