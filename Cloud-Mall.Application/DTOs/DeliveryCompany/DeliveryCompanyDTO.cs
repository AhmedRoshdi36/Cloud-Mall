namespace Cloud_Mall.Application.DTOs.DeliveryCompany
{
    public class DeliveryCompanyDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CommercialSerialNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
} 