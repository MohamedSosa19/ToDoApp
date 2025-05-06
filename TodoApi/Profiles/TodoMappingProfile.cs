using AutoMapper;
using TodoApi.DTOS;
using TodoApp.Core.Enums;
using TodoApp.Core.Models;

namespace TodoApi.Profiles
{
    public class TodoMappingProfile: Profile
    {
        public TodoMappingProfile()
        {
            // Map from Todo to TodoDto
            CreateMap<Todo, TodoDto>();

            // Map from CreateTodoDto to Todo
            CreateMap<CreateTodoDto, Todo>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => TodoStatus.Pending))
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Map from UpdateTodoDto to Todo
            CreateMap<UpdateTodoDto, Todo>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.LastModifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
