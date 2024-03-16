using AutoMapper;
using Cube.Application.Services;
using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.User.Dto;
using Cube.Core.Entities;
using Cube.Core.Models;
using Cube.Core.Models.Friendship;
using Cube.Core.Models.Messages;
using Cube.EntityFramework.Repository;

namespace Cube.Application.Utilities
{
    public static class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewChatDto, ChatEntity>()
                    .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title))
                    .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type));

                cfg.CreateMap<RegisterDto, UserEntity>()
                    .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Surname, act => act.MapFrom(src => src.Surname))
                    .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email))
                    .ForMember(dest => dest.Password, act => act.MapFrom(src => src.Password.GetHash()))
                    .ForMember(dest => dest.DateOfBirth, act => act.MapFrom(src => src.DateOfBirth));

                cfg.CreateMap<MessageEntity, MessageModel>()
                    .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Message, act => act.MapFrom(src => src.Message))
                    .ForMember(dest => dest.ChatId, act => act.MapFrom(src => src.ChatId))
                    .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.CreatedDate, act => act.MapFrom(src => src.CreatedDate))
                    .ForMember(dest => dest.UpdatedDate, act => act.MapFrom(src => src.UpdateDate));

                cfg.CreateMap<FriendshipEntity, FriendshipModel>()
                    .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                    .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.FriendId, act => act.MapFrom(src => src.FriendId));

                cfg.CreateMap<FriendshipDto, FriendshipEntity>()
                    .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.FriendId, act => act.MapFrom(src => src.FriendId));
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
