using Dapr.Client;
using CommandService.Data;
using CommandService.Models;
using CommandService.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CommandService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly DaprClient _daprClient;  // Injecter DaprClient

        public OrdersController(OrderService orderService, DaprClient daprClient)
        {
            _orderService = orderService;
            _daprClient = daprClient;  // Initialiser DaprClient
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            // Créer la commande dans le service
            var order = await _orderService.CreateOrderAsync(request.CustomerId, request.ProductId, request.Quantity);

            // Préparer le message à publier via Dapr
            var message = new
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            // Publier le message dans le sujet "product-stock-decreased"
            await _daprClient.PublishEventAsync("pubsub", "product-stock-decreased", message);

            // Retourner la réponse avec le nouvel ID de commande
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
        {
            await _orderService.UpdateOrderStatusAsync(id, status);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }
    }
}
