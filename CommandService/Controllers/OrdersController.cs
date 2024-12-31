using Dapr.Client;
using CommandService.Data;
using CommandService.Models;
using CommandService.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Prometheus;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CommandService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly DaprClient _daprClient;  // Injecter DaprClient

        // Définir un compteur pour les requêtes HTTP
        private static readonly Counter RequestCounter = Metrics.CreateCounter("orders_requests_total", 
            "Number of requests received by Orders API.", 
            new[] { "method", "status" });

        // Définir un histogramme pour la latence des requêtes
        private static readonly Histogram RequestDurationHistogram = Metrics.CreateHistogram("orders_request_duration_seconds", 
            "Histogram for the duration of HTTP requests to Orders API.");

        public OrdersController(OrderService orderService, DaprClient daprClient)
        {
            _orderService = orderService;
            _daprClient = daprClient;  // Initialiser DaprClient
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var stopwatch = Stopwatch.StartNew();
            try
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
            catch (Exception ex)
            {
                // Compter les erreurs sur cette route
                RequestCounter.Labels("POST", "500").Inc();
                throw new InvalidOperationException("Error creating order", ex);
            }
            finally
            {
                // Enregistrer la durée de la requête
                stopwatch.Stop();
                RequestDurationHistogram.Observe(stopwatch.Elapsed.TotalSeconds);
                RequestCounter.Labels("POST", "200").Inc();  // Compter une réponse réussie
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] string status)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _orderService.UpdateOrderStatusAsync(id, status);
                return NoContent();
            }
            catch (Exception ex)
            {
                RequestCounter.Labels("PUT", "500").Inc();
                throw new InvalidOperationException("Error updating order status", ex);
            }
            finally
            {
                stopwatch.Stop();
                RequestDurationHistogram.Observe(stopwatch.Elapsed.TotalSeconds);
                RequestCounter.Labels("PUT", "200").Inc();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                    return NotFound();

                return Ok(order);
            }
            catch (Exception ex)
            {
                RequestCounter.Labels("GET", "500").Inc();
                throw new InvalidOperationException("Error fetching order", ex);
            }
            finally
            {
                stopwatch.Stop();
                RequestDurationHistogram.Observe(stopwatch.Elapsed.TotalSeconds);
                RequestCounter.Labels("GET", "200").Inc();
            }
        }
    }
}
