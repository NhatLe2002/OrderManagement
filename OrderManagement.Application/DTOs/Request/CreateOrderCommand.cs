using OrderManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.Request
{
    public class CreateOrderCommand
    {
        public string CustomerName { get; set; }
        public List<OrderDetailComman> OrderDetails { get; set; } = new();

    }
    public class OrderDetailComman
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
