namespace Cloud_Mall.Application.DTOs.Order
{
    public class CustomerOrderDetailsDto
    {
        public int Id { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public string OverallStatus { get; set; }
        public List<VendorOrderDto> VendorOrders { get; set; }
    }
}