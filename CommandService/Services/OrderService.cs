using CommandService.Data;
using CommandService.Repositories;

namespace CommandService.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateOrderAsync(int customerId, int productId, int quantity)
        {
            var order = new Order
            {
                CustomerId = customerId,
                ProductId = productId,
                Quantity = quantity,
                Status = "En attente",
                OrderDate = DateTime.Now
            };

            // Enregistrer la commande dans la base de données
            await _orderRepository.CreateOrderAsync(order);

            return order;
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Commande non trouvée");
            }

            // Mettre à jour le statut de la commande
            order.Status = status;
            await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }
    }

}
