using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public bool Register(UserEntity user);
        public bool LoginUser(LoginModel login);
        public UserEntity GetUserByEmail(string email);
        public bool CheckEmailPassword(string email, string password, UserEntity result);
        public bool ForgetPassword(string email);
        public bool ResetPassword(string email, string newPassword);
        public bool VerifyPassword(string enteredPassword, string storedPassword);
        public bool SendResetEmail(string email, string token);
        Task<IEnumerable<UserEntity>> GetUsersAsync();

    }
}
