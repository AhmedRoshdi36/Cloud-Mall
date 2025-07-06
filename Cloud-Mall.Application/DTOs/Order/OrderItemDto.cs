namespace Cloud_Mall.Application.DTOs.Order
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtTimeOfPurchase { get; set; }
    }
}