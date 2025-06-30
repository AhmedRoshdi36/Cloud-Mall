namespace Cloud_Mall.Application.DTOs.Product
{
    public class ProductDto
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

        public int StoreID { get; set; }
        public string StoreName { get; set; }

        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }

        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
