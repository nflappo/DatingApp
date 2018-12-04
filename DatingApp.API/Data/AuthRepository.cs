using System;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string userName, string password)
        {
           var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == userName);

           if (user == null)
           {
               return null;
           }

           if(!VerifyPasswordHash(user.PasswordHash, password, user.PasswordSalt))
           {
               return null;
           }
           return user;

        }

        public async Task<User> Register(User user, string password)
        {
            byte [] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            await  _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UserExists(string userName)
        {
            if(await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName) == null)
            {
                return false;
            }
            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }            
        }

        private bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            return array1.SequenceEqual(array2);
        }

        private bool VerifyPasswordHash(byte[] existingPasswordHash,string password, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return CompareByteArrays(existingPasswordHash, computedHash);
            }
        }

    }
}