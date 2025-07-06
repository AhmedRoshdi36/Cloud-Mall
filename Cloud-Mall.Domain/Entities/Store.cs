namespace Cloud_Mall.Domain.Entities
{
    public class Store
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LogoURL { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = false;
        public string VendorID { get; set; }
        public virtual ApplicationUser Vendor { get; set; }

        public int StoreCategoryID { get; set; }
        public virtual StoreCategory StoreCategory { get; set; }

        public virtual ICollection<StoreAddress> Addresses { get; set; } = new List<StoreAddress>();
        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
