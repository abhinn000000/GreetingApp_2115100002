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

        public string greet()
        {
            return "Hello, World!!";
        }

        public string UserGreet(UserGreetModel usergreet)
        {
            return _greetingRL.Greeting(usergreet);
        }
        public bool GreetMessage(GreetingModel greetModel)
        {
            return _greetingRL.GreetMessage(greetModel);
        }
        public GreetingModel GetGreetingById(int id)
        {
            return _greetingRL.GetGreetingById(id);
        }
        public List<GreetingModel> GetAllGreetings()
        {
            var entityList = _greetingRL.GetAllGreetings();
            if (entityList != null)
            {
                return entityList.Select(g => new GreetingModel
                {
                    Id = g.Id,
                    GreetingMessage = g.Greeting
                }).ToList();
            }
            return null;
        }
        public GreetingModel EditGreeting(int id, GreetingModel greetingModel)
        {
            var result = _greetingRL.EditGreeting(id, greetingModel); // Calling Repository Layer
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
    }
}