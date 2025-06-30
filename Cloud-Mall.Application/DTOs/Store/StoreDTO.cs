namespace Cloud_Mall.Application.DTOs.Store
{
    public class StoreDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LogoURL { get; set; }
        public string CategoryName { get; set; }
        public List<GetStoreAddressDTO> Addresses { get; set; }
    }
}
