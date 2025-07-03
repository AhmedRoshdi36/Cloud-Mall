namespace Cloud_Mall.Application.DTOs.Store;
public class GetAllStoresDTO
{
    public int ID { get; set; }
    public string VendorID { get; set; }
    public string Name { get; set; }
    public string LogoURL { get; set; }
    public string Description { get; set; }
    public string CategoryName { get; set; }
}

public class GetAllStoresWithPaginationDTO
{
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int TotalNumberOfPages { get; set; }
    public List<GetAllStoresDTO> AllStores { get; set; }
}
