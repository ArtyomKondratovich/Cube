using AutoMapper;
using Cube.Application.Services;
using Cube.Application.Services.Chat.Dto;
using Cube.Application.Services.User.Dto;
using Cube.Core.Models;
using Cube.Core.Models.User;
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

                cfg.CreateMap<NewUserDto, UserEntity>()
                    .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Surname, act => act.MapFrom(src => src.Surname))
                    .ForMember(dest => dest.DateOfBirth, act => act.MapFrom(src => src.DateOfBirth));

                cfg.CreateMap<RegisterDto, AccountEntity>()
                    .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email))
                    .ForMember(dest => dest.PasswordHash, act => act.MapFrom(src => src.Password.GetHash()));

                cfg.CreateMap<RegisterDto, NewUserDto>()
                    .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Surname, act => act.MapFrom(src => src.Surname))
                    .ForMember(dest => dest.DateOfBirth, act => act.MapFrom(src => src.DateOfBirth));
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
