using DAL.Entities;
using ProductOrderManagement.Models;
using ServiceLayer.Models;

namespace ServiceLayer.IServices
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(CreateProductRequest request);
        Task<Product> UpdateProductAsync(UpdateProductRequest request);
        Task DeleteProductAsync(int id);
        Task<Product> UpdateProductStockAsync(UpdateProductStockRequest updateProductStockRequest);
    }
}