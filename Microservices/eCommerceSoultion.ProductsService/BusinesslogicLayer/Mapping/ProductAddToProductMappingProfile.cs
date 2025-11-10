using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinesslogicLayer.DTO;
using DataAccessLayer.Entities;

namespace BusinesslogicLayer.Mapping
{
    public class ProductAddToProductMappingProfile : Profile
    {
        public ProductAddToProductMappingProfile()
        {
            CreateMap<ProductAddRequest, Product>()
        .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
        .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.quantity ?? 0))  
        .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice ?? 0))
        .ForMember(dest => dest.ProductID, opt => opt.Ignore())
        .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryOptions.ToString()));


        }
    }
}
