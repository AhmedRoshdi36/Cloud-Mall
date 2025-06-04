namespace Cloud_Mall.Domain.Entities
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
