using System;
using System.Linq;
using AutoMapper;
using CloudinaryDotNet.Actions;
using DatingApp.API.Helpers;
using DatingApp.API.Models.DTOs;

namespace DatingApp.API.Models.Profiles
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<Value, ValueDTO>();
            CreateMap<Photo,PhotoForDetailedDTO>();
            CreateMap<User, UserForDetailedDTO>()
                .ForMember(dest => dest.PhotoURL, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt => 
                {
                   opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
                });
            CreateMap<User, UserForListDTO>()
                .ForMember(dest => dest.PhotoURL, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt => 
                {
                    opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
                });
            CreateMap<UserForUpdateDTO, User>();
            CreateMap<Photo, PhotoToReturnDTO>();
            CreateMap<PhotoForCreationDTO, Photo>();
            CreateMap<UserForRegisterDTO, User>();
        }
    }
}
