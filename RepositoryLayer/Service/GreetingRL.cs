using RepositoryLayer.Interface;
using System;
using ModelLayer.Model;
using NLog;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using Microsoft.EntityFrameworkCore;

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
            if (string.IsNullOrWhiteSpace(greetModel.GreetingMessage))
            {
                throw new ArgumentException("Greeting message cannot be empty.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Email == greetModel.UserEmail);
            if (user == null)
            {
                throw new Exception($"User with Email {greetModel.UserEmail} does not exist.");
            }

            if (_context.GreetMessages.Any(greet => greet.Greeting == greetModel.GreetingMessage
                                                     && greet.UserId == user.UserId))
            {
                return false;
            }

            var greetingEntity = new GreetingEntity
            {
                Greeting = greetModel.GreetingMessage,
                UserId = user.UserId
            };

            _context.GreetMessages.Add(greetingEntity);
            _context.SaveChanges();
            return true;
        }


        public GreetingModel GetGreetingById(int ID, string email)
        {
            var entity = _context.GreetMessages
                                 .Include(g => g.User)
                                 .FirstOrDefault(g => g.Id == ID && g.User != null && g.User.Email == email);

            if (entity != null)
            {
                return new GreetingModel()
                {
                    Id = entity.Id,
                    GreetingMessage = entity.Greeting,
                    UserEmail = entity.User?.Email,    // Null-check ensures no exception
                    UserName = $"{entity.User?.FirstName} {entity.User?.LastName}"
                };
            }
            return null;
        }



        public List<GreetingEntity> GetAllGreetings(string email)
        {
            return _context.GreetMessages
                           .Include(g => g.User) // Correct navigation property
                           .Where(g => g.User.Email == email) // Access email via the User navigation property
                           .ToList();
        }


        public GreetingEntity EditGreeting(int id, GreetingModel greetingModel, string email)
        {
            var entity = _context.GreetMessages
                                 .Include(g => g.User)
                                 .FirstOrDefault(g => g.Id == id && g.User.Email == email);

            if (entity != null)
            {
                entity.Greeting = greetingModel.GreetingMessage;
                _context.GreetMessages.Update(entity);
                _context.SaveChanges();
                return entity;
            }
            return null; // Greeting not found or unauthorized access
        }


        public bool DeleteGreeting(int id, string email)
        {
            var entity = _context.GreetMessages
                                 .Include(g => g.User)
                                 .FirstOrDefault(g => g.Id == id && g.User.Email == email);

            if (entity != null)
            {
                _context.GreetMessages.Remove(entity);
                _context.SaveChanges();
                return true; // Successfully deleted
            }
            return false; // Greeting not found or unauthorized access
        }


        public string Greeting(UserGreetModel usergreet)
        {
            string greetingMessage = string.Empty;

            if (!string.IsNullOrEmpty(usergreet.FirstName) && !string.IsNullOrEmpty(usergreet.LastName))
            {
                greetingMessage = $"Hello {usergreet.FirstName} {usergreet.LastName}";
            }
            else if (!string.IsNullOrEmpty(usergreet.FirstName))
            {
                greetingMessage = $"Hello {usergreet.FirstName}";
            }
            else if (!string.IsNullOrEmpty(usergreet.LastName))
            {
                greetingMessage = $"Hello {usergreet.LastName}";
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
