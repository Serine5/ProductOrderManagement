using DAL.Context;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly ApplicationDbContext _context;
        public IProductRepository ProductRepository { get; }
        public IOrderRepository OrderRepository { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ProductRepository = new ProductRepository(context);
            OrderRepository = new OrderRepository(context);
        }

        public async Task<int> CompleteAsync() =>
            await _context.SaveChangesAsync();

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