using Cloud_Mall.Application.DTOs.GoverningLocation;

namespace Cloud_Mall.Application.DTOs.Store
{
    internal class GetStoreAddressesDTO
    {
        public string Address { get; set; }
        public string Note { get; set; }
        public GoverningLocationDTO Location { get; set; }
    }
}
