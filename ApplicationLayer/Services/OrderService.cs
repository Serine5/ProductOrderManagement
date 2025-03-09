using DAL.Entities;
using DAL.Repositories;
using ProductOrderManagement.Models;
using ServiceLayer.IServices;

namespace ServiceLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId) =>
            await _unitOfWork.OrderRepository.GetOrdersByUserAsync(userId);

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request, string userId)
        {
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                UserId = userId,
                OrderItems = request.OrderItems.Select(oi => new OrderItem
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity
                }).ToList()
            };

            await _unitOfWork.OrderRepository.AddOrderAsync(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}