using System;

namespace Core.API.Dtos
{
    public class MessageCreationDto
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.Now;
        public string Content { get; set; }
    }
}