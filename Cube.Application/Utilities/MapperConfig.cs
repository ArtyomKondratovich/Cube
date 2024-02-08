using AutoMapper;
using Cube.Application.Services.Chat.Dto;
using Cube.Core.Models;
using Cube.EntityFramework.Repository;

namespace Cube.Application.Utilities
{
    public static class MapperConfig
    {
        public static Mapper InitializeAutomapper(IRepositoryWrapper wrapper)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewChatDto, ChatEntity>()
                    .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title))
                    .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type));
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
