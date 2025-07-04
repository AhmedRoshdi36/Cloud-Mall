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

public class GetAllProductsWithPaginationDTO
{
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int TotalNumberOfPages { get; set; }
    public List<GetAllProductsDTO> AllProducts { get; set; }
}