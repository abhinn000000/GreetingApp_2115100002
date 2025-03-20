using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public bool Register(UserEntity user);
        public UserEntity GetUserByEmail(string email);
        public bool ForgetPassword(string email);
        public bool ResetPassword(string email, string newPassword);
        public string HashPassword(string password);

        Task<IEnumerable<UserEntity>> GetUsersAsync();
    }
}