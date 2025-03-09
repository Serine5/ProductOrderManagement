using DAL.Entities;
using DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using ProductOrderManagement.Models;
using ServiceLayer.IServices;
using ServiceLayer.Models;

namespace ServiceLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private const string ProductsCacheKey = "Products";

        public ProductService(IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            if (_cache.TryGetValue(ProductsCacheKey, out IEnumerable<Product> products))
            {
                return products;
            }

            products = await _unitOfWork.ProductRepository.GetAllAsync();
            // Cache products for 5 minutes
            _cache.Set(ProductsCacheKey, products, TimeSpan.FromMinutes(5));
            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int id) =>
            await _unitOfWork.ProductRepository.GetByIdAsync(id);

        public async Task<Product> CreateProductAsync(CreateProductRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Description = request.Description ?? string.Empty,
                Price = request.Price,
                Quantity = request.Quantity,
                Category = request.Category
            };

            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            // Clear cached products on any change
            _cache.Remove(ProductsCacheKey);
            return product;
        }

        public async Task<Product> UpdateProductAsync(UpdateProductRequest request)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
            if (product == null)
                throw new Exception("Product not found");

            product.Name = request.Name;
            product.Description = request.Description ?? string.Empty;
            product.Price = request.Price;
            product.Quantity = request.Quantity;
            product.Category = request.Category;

            await _unitOfWork.ProductRepository.UpdateAsync(product);
            await _unitOfWork.CompleteAsync();
            _cache.Remove(ProductsCacheKey);
            return product;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");

            await _unitOfWork.ProductRepository.DeleteAsync(product);
            await _unitOfWork.CompleteAsync();
            _cache.Remove(ProductsCacheKey);
        }

        public async Task<Product> UpdateProductStockAsync(UpdateProductStockRequest request)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            product.Quantity = request.NewQuantity;
            await _unitOfWork.ProductRepository.UpdateProductStockAsync(request.ProductId, request.NewQuantity);
            _cache.Remove(ProductsCacheKey);
            return product;
        }
    }
}