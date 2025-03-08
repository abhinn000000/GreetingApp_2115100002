using System.Security.Cryptography;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Services
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 10000;
        public UserBL(IUserRL userRL)
        {
            _userRL = userRL;
        }

        public bool Register(UserEntity user)
        {
            return _userRL.Register(user);
        }
        public bool LoginUser(LoginModel login)
        {
            var user = GetUserByEmail(login.Email);
            if (user == null)
            {
                return false;
            }
            return CheckEmailPassword(login.Email, login.Password, user);
        }

        public UserEntity GetUserByEmail(string email)
        {
            return _userRL.GetUserByEmail(email);
        }
        public bool CheckEmailPassword(string email, string password, UserEntity result)
        {
            if (result == null)
            {
                return false;
            }
            return result.Email == email && VerifyPassword(password, result.Password);
        }
        public bool VerifyPassword(string enteredPassword, string storedPassword) // method to unhash the password stored in the DB
        {
            byte[] hashBytes = Convert.FromBase64String(storedPassword);
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, Iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);
                for (int i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i])
                        return false;
                }
            }
            return true;
        }
        public bool ForgetPassword(string email) => _userRL.ForgotPassword(email);
        public bool ResetPassword(string email, string newPassword)
        {
            return _userRL.ResetPassword(email, newPassword);
        }
    }
}