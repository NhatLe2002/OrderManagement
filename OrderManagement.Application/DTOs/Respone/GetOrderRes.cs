using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs.Respone
{
    public class GetOrderRes
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } 
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
        public List<OrderDetailRes> OrderDetails { get; set; } 

    }
}
