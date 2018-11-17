using System;

namespace DatingApp.API.Models.DTOs
{
    public class UserForListDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Gender {get; set;}
        public string KnownAs {get;set;}
        public int Age {get; set;}
        public DateTime CreateDate {get; set;}
        public DateTime LastActive {get; set;}
        public string City {get; set;}
        public string PhotoURL {get; set;}
    }
}