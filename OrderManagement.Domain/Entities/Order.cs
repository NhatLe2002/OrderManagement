using OrderManagement.Domain.Enums;
using OrderManagement.Domain.Errors;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Entities
{
    public class Order
    {
        public Order()
        {

        }

        public Order(string customerName, decimal totalAmount, OrderStatusEnum status, DateTime createdAt, DateTime updatedAt, List<OrderDetail> orderDetails)
        {
            CustomerName = customerName;
            TotalAmount = totalAmount;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            OrderDetails = orderDetails;
        }
        [Key]
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<OrderDetail> OrderDetails { get; set; } = new();
        public bool IsDeleted { get; set; } = false;
        public static Result<Order> Create(string customerName, List<OrderDetail> orderDetails)
        {
            //if (string.IsNullOrWhiteSpace(customerName))
            //{
            //    return Result<Order>.Fail("Customer name is required");
            //}

            //if (orderDetails == null || orderDetails.Count == 0)
            //{
            //    return Result<Order>.Fail("Order details are required");
            //}

            var totalAmount = orderDetails.Sum(x => x.Price * x.Quantity);


            var order = new Order
            {
                CustomerName = customerName,
                TotalAmount = totalAmount,
                OrderDetails = orderDetails
            };
            order.OrderDetails.AddRange(orderDetails);
            return order;
        }
        public async Task<Result<Order>> AddOrderDetail(OrderDetail orderDetail)
        {

            var visitDetailAdd = new OrderDetail(orderDetail.ProductName, orderDetail.Quantity, orderDetail.Price, this);

            OrderDetails.Add(visitDetailAdd);
            return this;
        }
        public Result<Order> Update(string customerName, List<OrderDetail> orderDetails)
        {


            if (orderDetails == null || orderDetails.Count == 0)
            {
                return Result.Failure<Order>(Error.OrderDetailNull);
            }

            CustomerName = customerName;
            UpdatedAt = DateTime.UtcNow;

            OrderDetails.RemoveAll(od => !orderDetails.Any(d => d.Id == od.Id));

            foreach (var detail in orderDetails)
            {
                var existingDetail = OrderDetails.FirstOrDefault(d => d.Id == detail.Id);
                if (existingDetail != null)
                {
                    existingDetail.Update(detail.ProductName, detail.Quantity, detail.Price);
                }
                else
                {
                    OrderDetails.Add(new OrderDetail(detail.ProductName, detail.Quantity, detail.Price, this));
                }
            }

            TotalAmount = OrderDetails.Sum(d => d.Price * d.Quantity);

            return this;
        }


        public Result<OrderDetail> AddOrderDetail(string productName, int quantity, decimal price)
        {

            var newDetail = new OrderDetail(productName, quantity, price, this);

            OrderDetails.Add(newDetail);

            TotalAmount = OrderDetails.Sum(d => d.Price * d.Quantity);

            return newDetail;
        }
        public void Delete()
        {
            IsDeleted = true;
        }
        


    }
}
