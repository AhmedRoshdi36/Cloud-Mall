namespace Cloud_Mall.Application.DTOs.Cart;

public class CartProductDTO
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string ImageURL { get; set; }
    public decimal Price { get; set; }
    public decimal? Discount { get; set; }
    public string StoreName { get; set; }
    public int Quantity { get; set; }
} 