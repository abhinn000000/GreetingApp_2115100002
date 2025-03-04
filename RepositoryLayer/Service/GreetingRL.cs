using RepositoryLayer.Interface;
using System;
using ModelLayer.Model;
using NLog;

namespace RepositoryLayer.Services
{
    public class GreetingRL : IGreetingRL
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public string Greeting(UserGreetModel usergreet)
        {
            string greetingMessage = string.Empty;

            if (!string.IsNullOrEmpty(usergreet.FirstName) && !string.IsNullOrEmpty(usergreet.LastName))
            {
                greetingMessage = $"Hello  {usergreet.FirstName} {usergreet.LastName}";
            }
            else if (!string.IsNullOrEmpty(usergreet.FirstName))
            {
                greetingMessage = $"Hello  {usergreet.FirstName}";
            }
            else if (!string.IsNullOrEmpty(usergreet.LastName))
            {
                greetingMessage = $"Hello  {usergreet.LastName}";
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