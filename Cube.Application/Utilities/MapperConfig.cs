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
                cfg.CreateMap<NewChatDto, ChatModel>()
                    .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title))
                    .ForMember(dest => dest.ChatAdmin, act => act.MapFrom(src => wrapper.UserRepository.GetUserById(src.AdminId??-1)))
                    .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type))
                    .ForMember(dest => dest.Participants, act => act.MapFrom(src => wrapper.ChatRepository.GetEntitiesByIds(src.PatricipantsIds)));


            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
