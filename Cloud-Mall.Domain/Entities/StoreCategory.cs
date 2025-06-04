namespace Cloud_Mall.Domain.Entities
{
    public class StoreCategory
    {
        public int StoreCategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Store> Stores { get; set; } = new List<Store>();
    }
}
