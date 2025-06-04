namespace Cloud_Mall.Domain.Entities
{
    public class Complaint
    {
        public int ComplaintID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
