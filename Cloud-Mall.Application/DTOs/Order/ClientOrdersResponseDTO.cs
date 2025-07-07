namespace Cloud_Mall.Application.DTOs.Order
{
    public class ClientOrdersResponseDTO
    {
        public int TotalOrders { get; set; }
        public List<CustomerOrderDetailsDto> Orders { get; set; }
    }
}