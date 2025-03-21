using AutoMapper;
using OrderManagement.Application.DTOs.Request;
using OrderManagement.Application.DTOs.Respone;
using OrderManagement.Application.Services.Interface;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Errors;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepo;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IOrderRepository orderRepo)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderRepo = orderRepo;
        }
        public async Task<Result<CreateOrderCommand>> CreateOrder(CreateOrderCommand command)
        {
            var orderDetail = _mapper.Map<List<OrderDetail>>(command.OrderDetails);
            var orderCreate = Order.Create(command.CustomerName, orderDetail);
            if (orderCreate.IsFailure)
            {
                return Result.Failure<CreateOrderCommand>(orderCreate.Error);
            }
            var order = orderCreate.Value;
            //foreach (var item in command.OrderDetails)
            //{
            //    var orderDetail = _mapper.Map<OrderDetail>(item);
            //    var orderDetailResult = await order.AddOrderDetail(orderDetail);
            //    if (orderDetailResult.IsFailure)
            //    {
            //        return Result.Failure<CreateOrderCommand>(orderDetailResult.Error);
            //    }
            //}
            await _orderRepo.AddAsync(order);
            var commit = await _unitOfWork.CommitAsync();
            if (!commit)
            {
                return Result.Failure<CreateOrderCommand>(Error.CommitError);
            }
            return command;
        }

      
        public async Task<Result<List<GetOrderRes>>> GetAllOrderAsync(int pageSize, int pageNumber)
        {
            var order = await _orderRepo.FindAsync(
                       s => true,
                       pageSize,
                       pageNumber,
                       includeProperties: "OrderDetails"
                   );

            if (!order.Any())
            {
                return Result.Failure<List<GetOrderRes>>(Error.OrderNotFound);
            }

            var res = _mapper.Map<List<GetOrderRes>>(order);

            return res;
        }

        public async Task<Result<GetOrderRes>> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);

            if (order == null)
            {
                return Result.Failure<GetOrderRes>(Error.OrderNotFound);
            }
            var orderRes = _mapper.Map<GetOrderRes>(order);
            
            return orderRes;
        }

        public async Task<Result<GetOrderRes>> UpdateOrder(int id, UpdateOrderCommand command)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
            {
                return Result.Failure<GetOrderRes>(Error.OrderNotFound);
            }
            var orderUpdate = _mapper.Map<Order>(command);

            var updateResult = order.Update(command.CustomerName, orderUpdate.OrderDetails);
            if (!await _orderRepo.UpdateAsync(updateResult.Value))
            {
                return Result.Failure<GetOrderRes>(Error.OrderUpdateError);
            }
            if (!await _unitOfWork.CommitAsync())
            {
                return Result.Failure<GetOrderRes>(Error.CommitError);
            }
            var result = _mapper.Map<GetOrderRes>(updateResult.Value);
            return result;
        }
        public async Task<Result<bool>> DeleteOrderAsync(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
            {
                return Result.Failure<bool>(Error.OrderNotFound);
            }
            order.Delete();
            if (!await _orderRepo.UpdateAsync(order))
            {
                return Result.Failure<bool>(Error.OrderDeleteError);
            }
            if (!await _unitOfWork.CommitAsync())
            {
                return Result.Failure<bool>(Error.CommitError);
            }
            return true;
        }

        public async Task<Result<GetOrderRes>> AddOrderDetailAsync(int orderId, OrderDetailCreateCommand command)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
            {
                return Result.Failure<GetOrderRes>(Error.OrderNotFound);
            }

            var resultAddOrderDetail = order.AddOrderDetail(command.ProductName, command.Quantity, command.Price);
            if (resultAddOrderDetail.IsFailure)
            {
                return Result.Failure<GetOrderRes>(resultAddOrderDetail.Error);
            }
            if (!await _orderRepo.UpdateAsync(order))
            {
                return Result.Failure<GetOrderRes>(Error.OrderUpdateError);
            }
            if (!await _unitOfWork.CommitAsync())
            {
                return Result.Failure<GetOrderRes>(Error.CommitError);
            }
            var result = _mapper.Map<GetOrderRes>(order);
            return result;
        }
    }
}
