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
    public class ProductToProductResponseMapping : Profile
    {
        public ProductToProductResponseMapping()
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.QuantityInStock))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice));

        }
    }
}
