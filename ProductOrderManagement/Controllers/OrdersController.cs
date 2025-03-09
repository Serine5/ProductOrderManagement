using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductOrderManagement.Models;
using ServiceLayer.IServices;
using System.Security.Claims;

namespace ProductOrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService) =>
            _orderService = orderService;

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orders = await _orderService.GetOrdersByUserAsync(userId!);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var order = await _orderService.CreateOrderAsync(request, userId);
            return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
        }
    }
}