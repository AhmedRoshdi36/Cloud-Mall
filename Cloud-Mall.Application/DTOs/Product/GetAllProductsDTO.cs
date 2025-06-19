namespace Cloud_Mall.Application.DTOs.Product;

public class GetAllProductsDTO
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public string ImagesURL { get; set; }
    public string CategoryName { get; set; }
    public double AverageRate { get; set; }
} 