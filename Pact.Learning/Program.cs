using System;

namespace Pact.Learning.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new TodosApiClient("http://localhost:9222");
            client.GetTodo("1");
            Console.WriteLine("Hello World!");
        }
    }
}
