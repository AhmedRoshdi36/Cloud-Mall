namespace Cloud_Mall.Application.DTOs.Order
{
    public class VendorStoreOrdersResponseDTO
    {
        public int TotalOrders { get; set; }
        public List<VendorOrderDto> Orders { get; set; }
    }
}