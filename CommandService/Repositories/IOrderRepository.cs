namespace CommandService.Repositories
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int orderId);
        Task UpdateOrderAsync(Order order);
    }
}
