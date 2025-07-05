namespace Cloud_Mall.Application.DTOs.Store;

using System.Collections.Generic;

public class GetOneStoreDTO
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string LogoURL { get; set; }
    public string CategoryName { get; set; }
    public List<GetStoreAddressDTO> Addresses { get; set; }
}