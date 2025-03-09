using DAL.Entities;

namespace DAL.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId);
        Task AddOrderAsync(Order order);
    }
}