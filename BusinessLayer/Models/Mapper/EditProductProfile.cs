using AutoMapper;
using BL.Models.VM;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models.Mapper
{
    public class EditProductProfile : Profile
    {
        public EditProductProfile()
        {
/*            CreateMap<ProductVM, Product>()
                .ForMember(dest => dest.AddedBy, opt => opt.MapFrom(src => src.AddedBy))
                .ForMember(dest => dest.AddedOn, opt => opt.MapFrom(src => src.AddedOn))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.AddedBy))
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(src => src.AddedOn))
                .ReverseMap();*/

            CreateMap<EditProductVM, Product>()
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src => src.ModifiedBy))
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(src => src.ModifiedOn))
                .ReverseMap();
        }
    }
}
