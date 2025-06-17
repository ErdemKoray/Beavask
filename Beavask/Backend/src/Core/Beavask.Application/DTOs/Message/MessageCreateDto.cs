namespace Beavask.Application.DTOs.Message
{
    public class MessageCreateDto
    {
        public string Content { get; set; } = string.Empty;
        public int ReceiverId { get; set; }
    }
}
