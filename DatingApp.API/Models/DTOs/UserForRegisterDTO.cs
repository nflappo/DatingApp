using System;
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
        [Required]
        public string Gender { get; set; }
        [Required]
        public string City {get; set;}
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set;}
        public DateTime CreateDate { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDTO()
        {
            CreateDate = DateTime.Now;
            LastActive = DateTime.Now;
        }

    }
}