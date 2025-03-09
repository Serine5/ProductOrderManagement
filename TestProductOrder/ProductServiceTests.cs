using DAL.Entities;
using DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using ProductOrderManagement.Models;
using ServiceLayer.IServices;
using ServiceLayer.Services;

namespace TestProductOrder
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IMemoryCache _cache;
        private readonly IProductService _productService;
        private const string ProductsCacheKey = "Products";

        public ProductServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _cache = new MemoryCache(new MemoryCacheOptions());

            _unitOfWorkMock.Setup(uow => uow.ProductRepository.GetAllAsync())
                .ReturnsAsync(new List<Product>
                {
                    new Product { Id = 1, Name = "Product1", Quantity = 10 },
                    new Product { Id = 2, Name = "Product2", Quantity = 20 }
                });

            _productService = new ProductService(_unitOfWorkMock.Object, _cache);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldCacheProducts()
        {
            var productsFirstCall = await _productService.GetAllProductsAsync();
            _unitOfWorkMock.Verify(uow => uow.ProductRepository.GetAllAsync(), Times.Once);

            //Should retrieve products from cache.
            var productsSecondCall = await _productService.GetAllProductsAsync();
            _unitOfWorkMock.Verify(uow => uow.ProductRepository.GetAllAsync(), Times.Once);

            //Verify that the cached products match the first call.
            Assert.Equal(productsFirstCall, productsSecondCall);
        }

        [Fact]
        public async Task CreateProductAsync_ShouldInvalidateCache()
        {
            await _productService.GetAllProductsAsync();
            Assert.True(_cache.TryGetValue(ProductsCacheKey, out var _));

            _unitOfWorkMock.Setup(uow => uow.ProductRepository.AddAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.CompleteAsync())
                .ReturnsAsync(1);

            var createRequest = new CreateProductRequest
            {
                Name = "Book",
                Description = "For beginners",
                Price = 50.00m,
                Quantity = 5,
                Category = "Programming"
            };

            var createdProduct = await _productService.CreateProductAsync(createRequest);

            Assert.False(_cache.TryGetValue(ProductsCacheKey, out var _));
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldInvalidateCache()
        {
            var existingProduct = new Product { Id = 1, Name = "Product1", Description = "Desc", Price = 10.0m, Quantity = 10, Category = "Cat" };
            _unitOfWorkMock.Setup(uow => uow.ProductRepository.GetByIdAsync(1))
                .ReturnsAsync(existingProduct);

            await _productService.GetAllProductsAsync();
            Assert.True(_cache.TryGetValue(ProductsCacheKey, out var _));

            var updateRequest = new UpdateProductRequest
            {
                Id = 1,
                Name = "Updated Product1",
                Description = "Updated Desc",
                Price = 15.0m,
                Quantity = 8,
                Category = "Updated Cat"
            };

            var updatedProduct = await _productService.UpdateProductAsync(updateRequest);

            Assert.Equal("Updated Product1", updatedProduct.Name);
            Assert.Equal("Updated Desc", updatedProduct.Description);
            Assert.Equal(15.0m, updatedProduct.Price);
            Assert.Equal(8, updatedProduct.Quantity);
            Assert.Equal("Updated Cat", updatedProduct.Category);

            //Cache should be invalidated.
            Assert.False(_cache.TryGetValue(ProductsCacheKey, out var _));
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldInvalidateCache()
        {
            var existingProduct = new Product { Id = 1, Name = "Product1", Quantity = 10 };
            _unitOfWorkMock.Setup(uow => uow.ProductRepository.GetByIdAsync(1))
                .ReturnsAsync(existingProduct);

            await _productService.GetAllProductsAsync();
            Assert.True(_cache.TryGetValue(ProductsCacheKey, out var _));

            await _productService.DeleteProductAsync(1);

            Assert.False(_cache.TryGetValue(ProductsCacheKey, out var _));
        }
        // Tests done duration 179 ms
    }
}