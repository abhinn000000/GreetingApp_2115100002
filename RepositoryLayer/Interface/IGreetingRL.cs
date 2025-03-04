using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        public string Greeting(UserGreetModel usergreet);

        public bool GreetMessage(GreetingModel greetModel);
    }
}
