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
    public class OrderItemToOrderItemResponseMapping : Profile
    {
        public OrderItemToOrderItemResponseMapping()
        {
            CreateMap<OrderItem,OrderItemResponse>()
                .ForMember(dest=>dest.ProductID,opt=>opt.MapFrom(src=>src.ProductID))
                .ForMember(dest=>dest.Quantity,opt=>opt.MapFrom(src=>src.Quantity))
                .ForMember(dest=>dest.UnitPrice,opt=>opt.MapFrom(src=>src.UnitPrice))
                .ForMember(dest=>dest.TotalPrice,opt=>opt.MapFrom(src=>src.TotalPrice))

                ;
        }
    }
}
