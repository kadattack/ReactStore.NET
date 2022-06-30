using PowerReact.DTOs;
using PowerReact.Entities;
using AutoMapper;

namespace PowerReact.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}