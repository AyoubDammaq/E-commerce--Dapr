namespace CommandService.Models
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
