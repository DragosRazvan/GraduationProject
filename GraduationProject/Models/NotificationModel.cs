namespace GraduationProject.Models
{
    public class NotificationModel
    {
        public int NotificationId { get; set; }
        public string NotificationMessage { get; set; }
        public DateTime NotificationTimeStamp { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
