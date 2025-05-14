namespace Beavask.Application.DTOs.Comment
{
    public class CommentCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int TaskId { get; set; }
    }
}
