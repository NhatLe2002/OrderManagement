using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderManagement.Application.DTOs.Request;
using OrderManagement.Application.DTOs.Respone;
using OrderManagement.Application.Services.Interface;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Errors;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly ILogger<OrderDetailService> _logger;

        public OrderDetailService(IUnitOfWork unitOfWork, IMapper mapper, IOrderDetailRepository orderDetailRepo, IOrderRepository orderRepo, ILogger<OrderDetailService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderDetailRepo = orderDetailRepo;
            _orderRepo = orderRepo;
            _logger = logger;
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

        public async Task<Result<List<OrderDetailRes>>> GetOrderDetailByOrderIdAsync(int orderId, int pageSize, int pageNumber)
        {
            var order = (await _orderRepo.FindAsync(
                      s => s.Id == orderId,
                      pageSize,
                      pageNumber,
                      includeProperties: "OrderDetails"
                  )).FirstOrDefault();

            if (order == null)
            {
                return Result.Failure<List<OrderDetailRes>>(Error.OrderNotFound);
            }

            var res = _mapper.Map<List<OrderDetailRes>>(order.OrderDetails);

            return res;
        }
        public async Task<Result<bool>> DeleteOrderDetailAsync(int orderDetailId)
        {

            _logger.LogInformation("Bắt đầu xóa OrderDetail với ID: {OrderDetailId}", orderDetailId);
            var orderDetail = await _orderDetailRepo.GetByIdAsync(orderDetailId);
            if (orderDetail == null)
            {
                _logger.LogWarning("Không tìm thấy OrderDetail với ID: {OrderDetailId}", orderDetailId);
                return Result.Failure<bool>(Error.OrderNotFound);
            }
            var order = (await _orderRepo.FindAsync(
                      s => s.OrderDetails.Any(s => s.Id == orderDetailId),
                      includeProperties: "OrderDetails"
                  )).FirstOrDefault();


            if (order.OrderDetails.Count <= 1 )
            {
                return Result.Failure<bool>(Error.LastOrderDetail);
            }

            await _orderDetailRepo.RemoveAsync(orderDetailId); 
            if (!await _unitOfWork.CommitAsync())
            {
                return Result.Failure<bool>(Error.CommitError);
            }
            return true;
            
        }

    }
}
