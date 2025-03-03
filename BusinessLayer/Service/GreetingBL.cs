using BusinessLayer.Interface;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Interface;
using System;

namespace BusinessLayer.Services
{
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
        }

        public string greeting(string firstName, string lastName)
        {
            try
            {
                if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
                {
                    _logger.Warn("Both First Name and Last Name are empty. Returning default greeting.");
                    return _greetingRL.Greeting(null, null);
                }

                _logger.Info($"Generating greeting for First Name: {firstName}, Last Name: {lastName}");
                return _greetingRL.Greeting(firstName, lastName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while generating greeting.");
                throw;
            }
        }
    }
}