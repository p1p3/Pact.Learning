using System;

using PactNet;
using PactNet.Mocks.MockHttpService;

namespace Pact.Learning.Tests.pacts
{
    // DECLARE THE PACT BETWEEN "CONSUMER" and "PROVIDER"
    public class ConsumerTodosApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort => 9222;

        public string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";

        public ConsumerTodosApiPact()
        {
            PactBuilder = new PactBuilder(new PactConfig { SpecificationVersion = "2.0.0" });

            PactBuilder
              .ServiceConsumer("Consumer")
              .HasPactWith("Todos API");

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}
