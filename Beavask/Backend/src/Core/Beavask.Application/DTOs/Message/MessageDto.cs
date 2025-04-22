namespace Beavask.Application.DTOs.Message
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
