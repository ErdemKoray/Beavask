namespace Beavask.Application.DTOs.Message
{
    public class MessageCreateDto
    {
        public string Content { get; set; } = string.Empty;
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
