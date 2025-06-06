namespace Cloud_Mall.Domain.Entities
{
    public class ProductCategory
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int StoreID { get; set; }
        public virtual Store Store { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
