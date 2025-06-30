namespace Cloud_Mall.Application.DTOs.Store
{
    public class StoreAddressDTO
    {
        public string StreetAddress { get; set; }
        public string Notes { get; set; }
        public int GoverningLocationID { get; set; }
    }

    public class GetStoreAddressDTO
    {
        public string StreetAddress { get; set; }
        public string Notes { get; set; }
        public string GoverningLocationName { get; set; }
    }
}
