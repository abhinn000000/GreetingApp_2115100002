using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Entity;
using HelloGreeting.Helper;
namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// User Controller to handle User Authentication APIs
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IUserBL _userBL;
        private readonly JwtTokenHelper _jwtTokenHelper;    
        public UserController(IUserBL userBL,JwtTokenHelper jwtTokenHelper)
        {
            _userBL = userBL;
            _jwtTokenHelper = jwtTokenHelper;
            _logger.Info("User Controller initialized successfully");
        }

        /// <summary>
        /// API to Register a New User
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public IActionResult Register(UserModel userModel)
        {
            var response = new ResponseModel<string>();
            try
            {
                UserEntity userEntity = new UserEntity()
                {
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    Email = userModel.Email,
                    Password = userModel.Password // Hashing will be done inside Business Layer
                };

                bool result = _userBL.Register(userEntity); // Now passing UserEntity instead of UserModel
                if (result)
                {
                    response.Success = true;
                    response.Message = "User Registered Successfully";
                    response.Data = userEntity.Email;
                    _logger.Info($"User Registered: {userModel.Email}");
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "User Already Exists";
                return Conflict(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in Register API: {ex.Message}");
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// API for User Login
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult Login(LoginModel loginModel)
        {
            var response = new ResponseModel<string>();
            try
            {
                _logger.Info("Login attempt for user: {0}", loginModel.Email);
                var user = _userBL.GetUserByEmail(loginModel.Email);

                if (user == null || !_userBL.CheckEmailPassword(loginModel.Email, loginModel.Password, user))
                {
                    _logger.Warn("Invalid login attempt for user: {0}", loginModel.Email);
                    return Unauthorized(new { Success = false, Message = "Invalid username or password." });
                }
                var token = _jwtTokenHelper.GenerateToken(user);
                _logger.Info("User {0} logged in successfully.", loginModel.Email);
                return Ok(new { Success = true, Message = "Login Successful.", Token = token }); // returning JWT token here
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Login failed.");
                return BadRequest(new { Success = false, Message = "Login failed.", Error = ex.Message });
            }
        }


        /// <summary>
        /// API to Handle Forget Password
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("ForgetPassword")]
        public IActionResult ForgetPassword(string email)
        {
            var response = new ResponseModel<string>();
            try
            {
                bool result = _userBL.ForgetPassword(email);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Reset Link Sent Successfully";
                    _logger.Info($"Reset Password Link Sent to: {email}");
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Email Not Found";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in ForgetPassword API: {ex.Message}");
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, response);
            }
        }

        /// <summary>
        /// API to Reset Password
        /// </summary>
        /// <param name="resetModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            var result = _userBL.ResetPassword(resetPasswordModel.Email, resetPasswordModel.NewPassword);
            ResponseModel<string> responseModel = new ResponseModel<string>();
            if (result == true)
            {
                responseModel.Success = true;
                responseModel.Message = "Password Reset Successfull.";
                responseModel.Data = "Your Password has been reset successfully";
                return Ok(responseModel);
            }
            responseModel.Success = false;
            responseModel.Message = "Password Reset Failed or Email Not Found";
            return BadRequest(responseModel);
        }


    }
}