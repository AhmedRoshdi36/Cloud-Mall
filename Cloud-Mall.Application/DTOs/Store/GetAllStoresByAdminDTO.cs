namespace Cloud_Mall.Application.DTOs.Store;
public class GetAllStoresByAdminDTO
{
    public int ID { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public string CategoryName { get; set; }
    public string VendorID { get; set; }
    public string VendorName { get; set; } 
    public string LogoURL { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<GetStoreAddressDTO> Addresses { get; set; }

}

public class GetAllStoresByAdminWithPaginationDTO
{
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int TotalNumberOfPages { get; set; }
    public List<GetAllStoresByAdminDTO> AllStores { get; set; }
}
