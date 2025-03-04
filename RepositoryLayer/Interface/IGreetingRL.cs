using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        public string Greeting(UserGreetModel usergreet);

        public bool GreetMessage(GreetingModel greetModel);

        public GreetingModel GetGreetingById(int ID);

        public List<GreetingEntity> GetAllGreetings();

        public GreetingEntity EditGreeting(int id, GreetingModel greetingModel);
        public bool DeleteGreeting(int id);
    }
}
