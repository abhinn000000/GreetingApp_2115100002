using System;

namespace ModelLayer.Model
{
    public class GreetingModel
    {
        public int Id { get; set; }
        public string GreetingMessage { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; } // Display User Details
    }
}
