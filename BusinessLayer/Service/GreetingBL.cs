using BusinessLayer.Interface;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public string Greet()
        {
            return "Hello, World!";
        }

        public string UserGreet(UserGreetModel usergreet)
        {
            return _greetingRL.Greeting(usergreet);
        }

        public bool GreetMessage(GreetingModel greetModel)
        {
            return _greetingRL.GreetMessage(greetModel);
        }

        public GreetingModel GetGreetingById(int id, string email)
        {
            return _greetingRL.GetGreetingById(id, email);
        }

        public GreetingModel EditGreeting(int id, GreetingModel greetingModel, string email)
        {
            var result = _greetingRL.EditGreeting(id, greetingModel, email);
            if (result != null)
            {
                return new GreetingModel()
                {
                    Id = result.Id,
                    GreetingMessage = result.Greeting
                };
            }
            return null;
        }

        public bool DeleteGreeting(int id, string email)
        {
            return _greetingRL.DeleteGreeting(id, email);
        }

        public List<GreetingModel> GetAllGreetings(string email)
        {
            var entityList = _greetingRL.GetAllGreetings(email);
            if (entityList != null)
            {
                return entityList.Select(g => new GreetingModel
                {
                    Id = g.Id,
                    GreetingMessage = g.Greeting,
                    UserEmail = g.User?.Email, // Ensure User data is included
                    UserName = g.User != null ? $"{g.User.FirstName} {g.User.LastName}" : null
                }).ToList();
            }
            return null;
        }

    }
}
