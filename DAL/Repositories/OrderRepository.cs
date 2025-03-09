using DAL.Context;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class OrderRepository : IOrderRepository, IAsyncDisposable
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(string userId) =>
            await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .ToListAsync();

        public async Task AddOrderAsync(Order order) =>
            await _context.Orders.AddAsync(order);

        private bool _disposed;

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (!_disposed)
            {
                if (_context != null)
                {
                    await _context.DisposeAsync();
                }
                _disposed = true;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            GC.SuppressFinalize(this);
        }
    }
}