using Cloud_Mall.Domain.Enums;

namespace Cloud_Mall.Application.DTOs.Order
{
    public class VendorOrderDto
    {
        public int Id { get; set; }
        public int CustomerOrderId { get; set; }
        public string StoreName { get; set; }
        public string OrderDate { get; set; }
        public decimal SubTotal { get; set; }
        public VendorOrderStatus Status { get; set; }

        // --- NEW PROPERTIES ---
        public string ClientName { get; set; }
        public string ShippingAddress { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }
    }
}