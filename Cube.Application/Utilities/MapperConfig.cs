using AutoMapper;
using Cube.Services.Services;
using Cube.Services.Services.Chat.Dto;
using Cube.Services.Services.Image.Dto;
using Cube.Services.Services.User.Dto;
using Cube.Core.Entities;
using Cube.Core.Models;
using Cube.Core.Models.Chat;
using Cube.Core.Models.Friendship;
using Cube.Core.Models.Image;
using Cube.Core.Models.Message;
using Cube.Core.Models.User;
using Cube.Repository.Repositories;

namespace Cube.Services.Utilities
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
                    .ForMember(dest => dest.UserId, act => act.MapFrom(src => src.FirstUserId))
                    .ForMember(dest => dest.FriendId, act => act.MapFrom(src => src.SecondUserId));

                cfg.CreateMap<FriendshipDto, FriendshipEntity>()
                    .ForMember(dest => dest.FirstUserId, act => act.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.SecondUserId, act => act.MapFrom(src => src.FriendId));

                cfg.CreateMap<NewImageDto, ImageEntity>()
                    .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type))
                    .ForMember(dest => dest.OwnerId, act => act.MapFrom(src => src.OwnerId));

                cfg.CreateMap<ImageEntity, ImageModel>()
                    .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type))
                    .ForMember(dest => dest.OwnerId, act => act.MapFrom(src => src.OwnerId));

                cfg.CreateMap<UserEntity, UserModel>()
                    .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Surname, act => act.MapFrom(src => src.Surname))
                    .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email))
                    .ForMember(dest => dest.DateOfBirth, act => act.MapFrom(src => src.DateOfBirth))
                    .ForMember(dest => dest.RoleId, act => act.MapFrom(src => src.RoleId));

                cfg.CreateMap<ChatEntity, ChatModel>()
                    .ForMember(dest => dest.Id, act => act.MapFrom(src => src.Id))
                    .ForMember(dest => dest.Title, act => act.MapFrom(src => src.Title))
                    .ForMember(dest => dest.Type, act => act.MapFrom(src => src.Type));
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
