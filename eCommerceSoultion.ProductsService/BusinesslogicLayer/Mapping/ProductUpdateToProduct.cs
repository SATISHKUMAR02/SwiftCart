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
    public class ProductUpdateToProduct : Profile
    {
        public ProductUpdateToProduct()
        {
            CreateMap<ProductUpdateRequest, Product>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.QuantityInStock, opt => opt.MapFrom(src => src.quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.ProductID, opt => opt.Ignore());
                
            
        }
    }
}
