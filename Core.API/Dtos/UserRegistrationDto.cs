using System.ComponentModel.DataAnnotations;

namespace Core.API.Dtos
{
    public class UserRegistrationDto
    {
        [Required]
        public string Username { get; set; }  

        [Required]
        [MinLength(6, ErrorMessage = "Please enter a password larger than 6 characters")]
        public string Password { get; set; }
    }
}