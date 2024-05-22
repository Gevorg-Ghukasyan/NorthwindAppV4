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
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<Product, ProductVM>()
           .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.StandartUnitCost))
           .ForMember(dest => dest.QuantityPerUnit, opt => opt.MapFrom(src => src.QuantityPerUnit)) 
           .ForMember(dest => dest.ProductCategoriesId, opt => opt.MapFrom(src => src.ProductCategoryId));

            CreateMap<ProductVM, Product>()
           .ForMember(dest => dest.StandartUnitCost, opt => opt.MapFrom(src => src.UnitPrice))
           .ForMember(dest => dest.QuantityPerUnit, opt => opt.MapFrom(src => src.QuantityPerUnit))
           .ForMember(dest => dest.ProductCategoryId, opt => opt.MapFrom(src => src.ProductCategoriesId))
           .ForMember(dest => dest.ProductCategory, opt => opt.Ignore()); 
        }
   
    }
}
