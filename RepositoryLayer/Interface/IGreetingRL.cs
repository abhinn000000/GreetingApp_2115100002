using ModelLayer.Model;
using RepositoryLayer.Entity;
using System.Collections.Generic;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        public string Greeting(UserGreetModel usergreet);

        public bool GreetMessage(GreetingModel greetModel);

        public GreetingModel GetGreetingById(int ID, string email); // Updated to include email
        List<GreetingEntity> GetAllGreetings(string email);         // Updated to include email

        public GreetingEntity EditGreeting(int id, GreetingModel greetingModel, string email); // Updated to include email
        public bool DeleteGreeting(int id, string email); // Updated to include email
    }
}
