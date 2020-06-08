using Newtonsoft.Json;
using PactNet;
using PactNet.Mocks.MockHttpService;
using PactNet.Models;
using System;
using System.Collections.Generic;


namespace Pact.Learning.Pacts
{

    public class ConsumerTodosApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort { get { return 9222; } }
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }

        public ConsumerTodosApiPact()
        {
            PactBuilder = new PactBuilder(new PactConfig { SpecificationVersion = "2.0.0" });

            PactBuilder
              .ServiceConsumer("Consumer")
              .HasPactWith("Todos API");

            MockProviderService = PactBuilder.MockService(MockServerPort); //Configure the http mock server
        }

        public void Dispose()
        {
            PactBuilder.Build(); //NOTE: Will save the pact file once finished
        }
    }
}
