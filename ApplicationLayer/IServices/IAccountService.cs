using ProductOrderManagement.Models;

namespace ServiceLayer.IServices
{
    public interface IAccountService
    {
        Task RegisterAsync(RegisterRequest request);
        Task<string> LoginAsync(LoginRequest request);
    }
}