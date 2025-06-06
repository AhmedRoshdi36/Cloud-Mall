namespace Cloud_Mall.Domain.Entities
{
    public class StoreAddress
    {
        public int ID { get; set; }
        public string StreetAddress { get; set; }
        public string Notes { get; set; }

        public int StoreID { get; set; }
        public virtual Store Store { get; set; }

        public int LocationID { get; set; }
        public virtual GoverningLocation GoverningLocation { get; set; }
    }
}
