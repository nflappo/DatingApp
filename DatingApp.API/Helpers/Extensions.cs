using System;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error",message);
            response.Headers.Add("Access-Control-Exposed-Header","Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin","*");

        }
        public static int CalculateAge(this DateTime dateOfBirth)
        {
            // Save today's date.
            var today = DateTime.Today;
            // Calculate the age.
            var age = today.Year - dateOfBirth.Year;
            // Go back to the year the person was born in case of a leap year
            if (dateOfBirth > today.AddYears(-age)) 
            {
                age--;
            }
            return age;

        }
    }
}