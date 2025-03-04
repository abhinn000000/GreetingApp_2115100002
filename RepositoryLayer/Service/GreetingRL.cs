using RepositoryLayer.Interface;
using System;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Services
{
    public class GreetingRL : IGreetingRL
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly GreetingContext _context;

        public GreetingRL(GreetingContext context)
        {
            _context = context;
        }
        public bool GreetMessage(GreetingModel greetModel)
        {
            if (_context.GreetMessages.Any(greet => greet.Greeting == greetModel.GreetingMessage))
            {
                return false;
            }
            var greetingEntity = new GreetingEntity
            {
                Greeting = greetModel.GreetingMessage,
            };
            _context.GreetMessages.Add(greetingEntity);
            _context.SaveChanges();
            return true;
        }
        public GreetingModel GetGreetingById(int ID)
        {
            var entity = _context.GreetMessages.FirstOrDefault(g => g.Id == ID);

            if (entity != null)
            {
                return new GreetingModel()
                {
                    Id = entity.Id,
                    GreetingMessage = entity.Greeting
                };
            }
            return null;
        }
        public GreetingEntity EditGreeting(int id, GreetingModel greetingModel)
        {
            var entity = _context.GreetMessages.FirstOrDefault(g => g.Id == id);
            if (entity != null)
            {
                entity.Greeting = greetingModel.GreetingMessage;
                _context.GreetMessages.Update(entity);
                _context.SaveChanges();
                return entity; // Returning the updated Entity
            }
            return null; // If not found
        }
        public List<GreetingEntity> GetAllGreetings()
        {
            return _context.GreetMessages.ToList();
        }
        public bool DeleteGreeting(int id)
        {
            var entity = _context.GreetMessages.FirstOrDefault(g => g.Id == id);
            if (entity != null)
            {
                _context.GreetMessages.Remove(entity);
                _context.SaveChanges();
                return true; 
            }
            return false; 
        }

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