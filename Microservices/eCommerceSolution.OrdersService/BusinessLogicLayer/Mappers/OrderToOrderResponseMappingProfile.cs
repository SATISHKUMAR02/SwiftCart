using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogicLayer.DTO;
using DataAccessLayer.Entites;

namespace BusinessLogicLayer.Mappers
{
    public class OrderToOrderResponseMappingProfile : Profile
    {
        public OrderToOrderResponseMappingProfile()
        {
            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.OrderID, opt => opt.MapFrom(src => src.OrderID))
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserID))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.TotalBill, opt => opt.MapFrom(src => src.TotalBill))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate));
                ;
        }

    }
}
