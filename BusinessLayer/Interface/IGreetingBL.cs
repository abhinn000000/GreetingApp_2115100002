﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        public string greet();
        public string UserGreet(UserGreetModel usergreet);

        public bool GreetMessage(GreetingModel greetModel);

        public GreetingModel GetGreetingById(int id);
        public List<GreetingModel> GetAllGreetings();
        public GreetingModel EditGreeting(int id, GreetingModel greetingModel);
        public bool DeleteGreeting(int id);
    }
}
