using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public bool Register(UserEntity user);
        public UserEntity GetUserByEmail(string email);
        //string Login(string email, string password);
        public string HashPassword(string password);
        public bool ForgotPassword(string email);
        public bool ResetPassword(string email, string newPassword);
    }
}