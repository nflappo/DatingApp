using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Models.DTOs
{
    public class UserForRegisterDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "No empty strings")]
        public string UserName { get; set; }
        [Required]
        [StringLength(8,MinimumLength = 4, ErrorMessage = "Password must be between 4 and 8 digits")]
        public string Password { get; set; }
    }
}