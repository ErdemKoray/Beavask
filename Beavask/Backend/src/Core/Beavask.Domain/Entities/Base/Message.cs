using System;

namespace Beavask.Domain.Entities.Base
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }  = string.Empty;
        public DateTime CreatedAt { get; set; }

        public required User Sender { get; set; }
        public int SenderId { get; set; }

        public required User Receiver { get; set; }
        public int ReceiverId { get; set; }

    }
}