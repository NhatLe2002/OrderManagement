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
    public interface IOrderService
    {
        public Task<Result<CreateOrderCommand>> CreateOrder(CreateOrderCommand command);
        public Task<Result<List<GetOrderRes>>> GetAllOrderAsync(int pageSize, int pageNumber);
        public Task<Result<GetOrderRes>> GetOrderByIdAsync(int orderId);
        public Task<Result<GetOrderRes>> UpdateOrder(int orderId, UpdateOrderCommand command);
        public Task<Result<bool>> DeleteOrderAsync(int orderId);

    }
}
