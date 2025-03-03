using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using BusinessLayer.Interface;

namespace HelloGreetingApplication.Controllers

{   /// <summary>
    /// Class providing API for HelloGreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private  readonly IGreetingBL _greetingBL; 

        public HelloGreetingController(IGreetingBL greetingBL) {
            _greetingBL = greetingBL;
            _logger.Info("Logger has been integrated");

        }

        [HttpGet]
        public IActionResult Get() {
            /// <summary>
            /// Method to Get the response from server 
            /// </summary>
            ResponseModel<string> responseModel = new ResponseModel<string>();
            _logger.Info("Get method executed");
            responseModel.Success = true;
            responseModel.Message = "Hello to Greeting App API Endpoint";
            responseModel.Data = "Hello world";
            return Ok(responseModel);
            
        }

        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            /// <summary>
            /// method to add an entry on the server 
            /// </summary>
            ResponseModel<string> responseModel = new ResponseModel<string>();
            _logger.Info("Post method executed");
            responseModel.Success = true;
            responseModel.Message = "Request Recieved Successfully";
            responseModel.Data = $"Key: {requestModel.key}, Value : {requestModel.value}";
            return Ok(responseModel);
        }

        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            /// <summary>
            /// method to update an already existing entry 
            /// </summary>
            ResponseModel<string> responseModel = new ResponseModel<string>();
            _logger.Info("Put method executed");
            responseModel.Success = true;
            responseModel.Message = "Data updated Successfully";
            responseModel.Data = $"Updated Key: {requestModel.key}, Updated Value : {requestModel.value}";
            return Ok(responseModel);
        }


        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {
            /// <summary>
            /// method to update a specific field of a resource
            /// </summary>
            ResponseModel<string> responseModel = new ResponseModel<string>();
            _logger.Info("Patch method executed");
            responseModel.Success = true;
            responseModel.Message = "The field was updated";
            responseModel.Data = $"Updated Key: {requestModel.key}, Updated Value : {requestModel.value}";
            return Ok(responseModel);
        }

        [HttpDelete]    
        public IActionResult Delete(RequestModel requestModel)
        {
            /// <summary>
            /// method to delete an entry
            /// </summary>
            ResponseModel<string> responseModel = new ResponseModel<string>();
            _logger.Info("Delete method executed");
            responseModel.Success = true;
            responseModel.Message = "The field was deleted";
            responseModel.Data = "Deletion successful";
            return Ok(responseModel);
        }

        [HttpGet]
        [Route("GetGreeting")]
        public IActionResult GetGreeting(string? firstName, string? lastName)
        {
            /// <summary>
            /// method to greet the user with first name,last name or both
            /// </summary>
            try
            {
                ResponseModel<string> responseModel = new ResponseModel<string>();
                _logger.Info("User has been greeted");
                string greetingMessage = string.Empty;

                if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
                {
                    greetingMessage = $"Hello {firstName} {lastName}";
                }
                else if (!string.IsNullOrEmpty(firstName))
                {
                    greetingMessage = $"Hello {firstName}";
                }
                else if (!string.IsNullOrEmpty(lastName))
                {
                    greetingMessage = $"Hello {lastName}";
                }
                else
                {
                    greetingMessage = "Hello World";
                }

                responseModel.Success = true;
                responseModel.Message = "Greeting Generated Successfully";
                responseModel.Data = greetingMessage;

                _logger.Info($"Greeting Message: {greetingMessage}");

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception Occurred: {ex.Message}");
                return StatusCode(500, "Something went wrong: " + ex.Message);
            }
        }
    }
}
