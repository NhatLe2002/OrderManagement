using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs.Request;
using OrderManagement.Application.Services;
using OrderManagement.Application.Services.Interface;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [Route("api/orders/{id}/order-details")]
        [HttpPost]
        public async Task<IActionResult> AddOrderDetail(int orderId, [FromBody] OrderDetailCreateCommand command)
        {
            var result = await _orderDetailService.AddOrderDetailAsync(orderId, command);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
        [Route("/api/orders/{id}/order-details")]
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails(int orderId, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            var result = await _orderDetailService.GetOrderDetailByOrderIdAsync(orderId, pageSize, pageNumber);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }

        [HttpDelete("api/order-details/{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var result = await _orderDetailService.DeleteOrderDetailAsync(id);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Value);
        }
    }
}
