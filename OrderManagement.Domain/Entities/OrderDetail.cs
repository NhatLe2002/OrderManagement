using OrderManagement.Domain.Errors;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Entities
{
    public class OrderDetail
    {
        public OrderDetail()
        {

        }

        public OrderDetail(string productName, int quantity, decimal price, Order order)
        {
            ProductName = productName;
            Quantity = quantity;
            Price = price;
            Order = order;
        }

        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public void Update(string productName, int quantity, decimal price)
        {
            ProductName = productName;
            Quantity = quantity;
            Price = price;
        }
        //public Result Delete()
        //{
        //    if (Order.OrderDetails.Count <= 1)
        //    {
        //        return Result.Failure<Order>(Error.OrderDetailNull);
        //    }

        //    return Result.Success();
        //}
        //public Result Delete(int orderDetailId)
        //{
        //    if (OrderDetails.Count == 1)
        //    {
        //        return Result.Failure<Order>(Error.OrderDetailNull);

        //    }

        //    var orderDetail = OrderDetails.FirstOrDefault(d => d.Id == orderDetailId);
        //    if (orderDetail == null)
        //    {
        //        return Result.Failure<Order>(Error.OrderDetailNotFound);

        //    }

        //    OrderDetails.Remove(orderDetail);

        //    TotalAmount = OrderDetails.Sum(d => d.Price * d.Quantity);

        //    return Result.Success();
        //}

    }

}
