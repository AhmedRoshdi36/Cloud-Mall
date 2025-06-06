namespace Cloud_Mall.Domain.Entities
{
    public class Cart
    {
        public int ID { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string ClientID { get; set; }
        public virtual ApplicationUser Client { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
