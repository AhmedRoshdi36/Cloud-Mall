namespace Cloud_Mall.Domain.Entities
{
    public class DeliveryCompany
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CommercialSerialNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
        
        public virtual ICollection<DeliveryOffer> Offers { get; set; } = new List<DeliveryOffer>();
    }
} 