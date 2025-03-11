using System.Collections.Generic;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        string Greet();
        string UserGreet(UserGreetModel usergreet);

        bool GreetMessage(GreetingModel greetModel);

        GreetingModel GetGreetingById(int id, string email);  // Updated to include email
        List<GreetingModel> GetAllGreetings(string email);    // Updated to include email

        GreetingModel EditGreeting(int id, GreetingModel greetingModel, string email); // Updated to include email

        bool DeleteGreeting(int id, string email); // Updated to include email
    }
}
