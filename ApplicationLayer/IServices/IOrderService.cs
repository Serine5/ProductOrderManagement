using DAL.Entities;
using ProductOrderManagement.Models;

namespace ServiceLayer.IServices
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId);
        Task<Order> CreateOrderAsync(CreateOrderRequest request, string userId);
    }
}