namespace Cloud_Mall.Domain.Entities
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public int Stock { get; set; }
        public string ImagesURL { get; set; }
        public bool IsDeleted { get; set; } = false;



        public int StoreID { get; set; }
        public virtual Store Store { get; set; }

        public int ProductCategoryID { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
