namespace Cloud_Mall.Domain.Entities
{
    public class Review
    {
        public int ReviewID { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ClientID { get; set; }
        public virtual ApplicationUser Client { get; set; }

        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}
