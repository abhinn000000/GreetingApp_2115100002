using BusinessLayer.Interface;
using BusinessLayer.Services;
using HelloGreetingApplication.Helper;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Entity;


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
        private readonly EmailService _emailService;


        public UserController(IUserBL userBL, JwtTokenHelper jwtTokenHelper, EmailService emailService)
        {
            _userBL = userBL;
            _logger.Info("User Controller initialized successfully");
            _jwtTokenHelper = jwtTokenHelper;
            _emailService = emailService;
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
                return Ok(new { Success = true, Message = "Login Successful.", Token = token });
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
                var user = _userBL.GetUserByEmail(email);
                if (user == null)
                {
                    _logger.Warn($"Forget Password request failed. Email not found: {email}");
                    response.Success = false;
                    response.Message = "Email Not Found";
                    return NotFound(response);
                }

                string token = _jwtTokenHelper.GenerateToken(user);
                if (string.IsNullOrEmpty(token))
                {
                    _logger.Error("JWT Token generation failed.");
                    response.Success = false;
                    response.Message = "Failed to generate reset token";
                    return StatusCode(500, response);
                }

                bool emailSent = _emailService.SendResetEmail(email, token);
                if (emailSent)
                {
                    response.Success = true;
                    response.Message = "Reset Link Sent Successfully";
                    _logger.Info($"Reset Password Link Sent to: {email}");
                    return Ok(response);
                }

                _logger.Error("Failed to send reset email.");
                response.Success = false;
                response.Message = "Failed to send reset email";
                return StatusCode(500, response);
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
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(string token, string newPassword)
        {
            var response = new ResponseModel<string>();
            try
            {
                // Validate the token and extract the email
                var email = _jwtTokenHelper.ValidateToken(token);
                if (string.IsNullOrEmpty(email))
                {
                    response.Success = false;
                    response.Message = "Invalid or Expired Token";
                    return Unauthorized(response);
                }

                // Reset Password
                bool result = _userBL.ResetPassword(email, newPassword);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Password Reset Successfully";
                    return Ok(response);
                }

                response.Success = false;
                response.Message = "Password Reset Failed";
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error in ResetPassword API: {ex.Message}");
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
                return StatusCode(500, response);
            }
        }



    }
}