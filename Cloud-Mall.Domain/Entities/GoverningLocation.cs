namespace Cloud_Mall.Domain.Entities
{
    public class GoverningLocation
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public virtual ICollection<StoreAddress> StoreAddresses { get; set; } = new List<StoreAddress>();
    }
}

