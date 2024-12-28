using CommandService.Data;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CommandDbContext _context;

        public OrderRepository(CommandDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order); 
            await _context.SaveChangesAsync();    
        }

        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            // Rechercher la commande par son ID avec la méthode FirstOrDefaultAsync
            var order = await _context.Orders
                                      .FirstOrDefaultAsync(o => o.Id == orderId);
            return order;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            // Rechercher la commande existante
            var existingOrder = await _context.Orders
                                              .FirstOrDefaultAsync(o => o.Id == order.Id);
            if (existingOrder != null)
            {
                // Mettre à jour les propriétés de l'ordre
                existingOrder.Status = order.Status;
    
                // Sauvegarder les changements
                await _context.SaveChangesAsync();
            }
            else
            {
                // Gérer le cas où la commande n'existe pas, par exemple lever une exception
                throw new KeyNotFoundException($"Order with ID {order.Id} not found.");
            }
        }
    }

}
