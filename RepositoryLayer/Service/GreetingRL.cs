using RepositoryLayer.Interface;
using System;
using ModelLayer.Model;
using NLog;

namespace RepositoryLayer.Services
{
    public class GreetingRL : IGreetingRL
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public string Greeting(string firstName, string lastName)
        {
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

            _logger.Info($"Generated Greeting: {greetingMessage}");
            return greetingMessage;
        }
    }
}