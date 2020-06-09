using System;

using PactNet;

namespace Pact.Learning.Publisher
{
    class Program
    {
        static void Main(string[] args) {
            try {
                var pactRoute = @"C:\Users\felja\Source\Repos\Pact.Learning\Pact.Learning.Tests\pacts\consumer-todos_api.json";
                var publisher = new PactPublisher("https://broker.pact.dius.com.au",
                                                  new PactUriOptions("broker_token"));
                publisher.PublishToBroker(pactRoute, "1.0.0");
                Console.WriteLine("Published");
                Console.ReadKey();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                Console.ReadKey();
                throw;
            }
            
        }
    }
}
