using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog;
using BusinessLayer.Interface;
using Middleware.GlobalExceptionHandler;
using Middleware.GlobalExceptionHandler;

namespace HelloGreetingApplication.Controllers

{   /// <summary>
    /// Class providing API for HelloGreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IGreetingBL _greetingBL;

        public HelloGreetingController(IGreetingBL greetingBL)
        {
            _greetingBL = greetingBL;
            _logger.Info("Logger has been integrated");

        }

        [HttpGet]
        public IActionResult Get()
        {
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
        [Route("HelloWorld")]
        public string GetHello()
        {
            return _greetingBL.greet();
        }

        [HttpPost]
        [Route("PostGreeting")]
        public IActionResult PostGreeting(UserGreetModel usergreet)
        {
            /// <summary>
            /// method to greet the user with first name,last name or both
            /// </summary>
            var result = _greetingBL.UserGreet(usergreet);
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Greeting Generated Successfully";
            responseModel.Data = result;

            _logger.Info("User has been greeted");

            return Ok(responseModel);
        }

        [HttpPost("greetmessage")]

        public IActionResult GreetMessage(GreetingModel greetModel)
        {
            var response = new ResponseModel<string>();
            bool isMessageGrret = _greetingBL.GreetMessage(greetModel);
            if (isMessageGrret)
            {
                response.Success = true;
                response.Message = "Greet Message!";
                response.Data = greetModel.ToString();
                return Ok(response);
            }
            response.Success = false;
            response.Message = "Greet Message Already Exist.";
            return Conflict(response);
        }

        [HttpGet("GetGreetingById/{id}")]
        public IActionResult GetGreetingById(int id)
        {
            var response = new ResponseModel<GreetingModel>();
            try
            {
                var result = _greetingBL.GetGreetingById(id);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Greeting Message Found";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Greeting Message Not Found";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpGet("GetAllGreetings")]
        public IActionResult GetAllGreetings()
        {
            ResponseModel<List<GreetingModel>> response = new ResponseModel<List<GreetingModel>>();
            try
            {
                var result = _greetingBL.GetAllGreetings();
                if (result != null && result.Count > 0)
                {
                    response.Success = true;
                    response.Message = "Greeting Messages Found";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "No Greeting Messages Found";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex);
                return StatusCode(500, errorResponse);
            }
        }

        [HttpPut("EditGreeting/{id}")]
        public IActionResult EditGreeting(int id, GreetingModel greetModel)
        {
            ResponseModel<GreetingModel> response = new ResponseModel<GreetingModel>();
            try
            {
                var result = _greetingBL.EditGreeting(id, greetModel);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Greeting Message Updated Successfully";
                    response.Data = result;
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Greeting Message Not Found";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex);
                return StatusCode(500, errorResponse);
            }
        }
        [HttpDelete("DeleteGreeting/{id}")]
        public IActionResult DeleteGreeting(int id)
        {
            ResponseModel<string> response = new ResponseModel<string>();
            try
            {
                bool result = _greetingBL.DeleteGreeting(id);
                if (result)
                {
                    response.Success = true;
                    response.Message = "Greeting Message Deleted Successfully";
                    return Ok(response);
                }
                response.Success = false;
                response.Message = "Greeting Message Not Found";
                return NotFound(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ExceptionHandler.CreateErrorResponse(ex);
                return StatusCode(500, errorResponse);
            }
        }
    }
}
