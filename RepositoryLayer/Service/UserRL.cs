using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System.Security.Cryptography;
using System.Text;
using RepositoryLayer.Context;
using NLog;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly GreetingContext context;
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;

        public UserRL(GreetingContext context)
        {
            this.context = context;
        }
        public bool Register(UserEntity user)
        {
            var isUserExist = context.Users.Any(u => u.Email == user.Email);
            if (isUserExist) return false;

            user.Password = HashPassword(user.Password);
            context.Users.Add(user);
            context.SaveChanges();
            return true;
        }

        //public string Login(string email, string password)
        //{
        //    var user = context.Users.FirstOrDefault(u => u.Email == email);
        //    if (user == null)
        //        return "Invalid Email";

        //    if (VerifyPassword(password, user.Password))
        //        return "Login Successful";

        //    return "Invalid Password";
        //}
        public UserEntity GetUserByEmail(string email)

        {
            return context.Users.FirstOrDefault(user => user.Email == email);
        }

        public bool ForgetPassword(string email)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return true;
            }
            return false;
        }

        public bool ResetPassword(string email, string newPassword)
        {
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.Password = HashPassword(newPassword);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);
                byte[] hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}