using AutoMapper;
using OrderManagement.Application.DTOs.Request;
using OrderManagement.Application.DTOs.Respone;
using OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {

            #region Order map
            CreateMap<CreateOrderCommand, Order>().ReverseMap();
            CreateMap<OrderDetailComman, OrderDetail>().ReverseMap();
            CreateMap<OrderDetailRes, OrderDetail>().ReverseMap();
            CreateMap<GetOrderRes, Order>().ReverseMap()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<UpdateOrderCommand, Order>()
             .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
            CreateMap<UpdateOrderDetailCommand, OrderDetail>().ReverseMap();
            #endregion
        }
    }
}
