using OrderManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.Request
{
    public class UpdateOrderCommand
    {
        public string CustomerName { get; set; } = string.Empty;
        public OrderStatusEnum Status { get; set; }
        public List<UpdateOrderDetailCommand> OrderDetails { get; set; } = new();

    }
    public class UpdateOrderDetailCommand
    {
        public int Id { get; set; } 
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
