namespace DAL.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        IOrderRepository OrderRepository { get; }
        Task<int> CompleteAsync();
    }
}