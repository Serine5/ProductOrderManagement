using DAL.Context;
using DAL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ProductRepository : IProductRepository, IAsyncDisposable
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _context.Products.ToListAsync();

        public async Task<Product?> GetByIdAsync(int id) =>
            await _context.Products.FindAsync(id);

        public async Task AddAsync(Product product) =>
            await _context.Products.AddAsync(product);

        public async Task UpdateAsync(Product product) =>
            _context.Products.Update(product);

        public async Task DeleteAsync(Product product) =>
            _context.Products.Remove(product);

        private bool _disposed;
        public async Task UpdateProductStockAsync(int productId, int newQuantity)
        {
            var productIdParam = new SqlParameter("@ProductId", productId);
            var newQuantityParam = new SqlParameter("@NewQuantity", newQuantity);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC UpdateProductStock @ProductId, @NewQuantity",
                productIdParam, newQuantityParam);
        }

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