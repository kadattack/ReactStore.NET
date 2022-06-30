using PowerReact.Entities.OrderAggregate;

namespace PowerReact.DTOs
{
    public class CreateOrderDto
    {
        public bool SaveAddress { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
    }
}