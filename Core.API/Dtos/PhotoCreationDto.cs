using System;
using Microsoft.AspNetCore.Http;

namespace Core.API.Dtos
{
    public class PhotoCreationDto
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
        public string PublicId { get; set; }
    }
}