using OrderManagement.Application.DTOs.Request;
using OrderManagement.Application.DTOs.Respone;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Services.Interface
{
    public interface IOrderDetailService
    {
        public Task<Result<GetOrderRes>> AddOrderDetailAsync(int orderId, OrderDetailCreateCommand command); 
        public Task<Result<List<OrderDetailRes>>> GetOrderDetailByOrderIdAsync(int orderId, int pageSize, int pageNumber);
        public Task<Result<bool>> DeleteOrderDetailAsync(int orderDetailId);
    }
}
