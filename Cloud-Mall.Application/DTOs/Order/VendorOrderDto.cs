using Cloud_Mall.Domain.Enums;

namespace Cloud_Mall.Application.DTOs.Order
{
    public class VendorOrderDto
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public decimal SubTotal { get; set; }
        public VendorOrderStatus Status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}