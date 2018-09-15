using AutoMapper;
using DatingApp.API.Models.DTOs;

namespace DatingApp.API.Models.Profiles
{
    public class ValueProfile : Profile
    {
        public ValueProfile()
        {
            CreateMap<Value, ValueDTO>();
        }
    }
}
