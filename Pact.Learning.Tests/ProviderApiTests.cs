using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Pact.Learning.Tests.Middlewares;
using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using System.Collections.Generic;
using System.Threading;

namespace Pact.Learning.Tests
{
    [TestClass]
    public class ProviderApiTests
    {
        private readonly WebApplicationFactory<Pact.Provider.Startup> _factory;
        private readonly CancellationTokenSource appCancellationToken = new CancellationTokenSource();
        const string serviceUri = "http://localhost:9222";
        public ProviderApiTests()
        {
            this._factory = new WebApplicationFactory<Pact.Provider.Startup>();


            var webHost = WebHost
                      .CreateDefaultBuilder(null)
                      .UseStartup<Pact.Provider.Startup>()
                      .ConfigureTestServices(sc =>
                      {
                          sc.AddTransient<IStartupFilter, PactStartUpFilter>();
                      })
                      .UseUrls(serviceUri).Build();


            webHost.RunAsync(appCancellationToken.Token);

        }

        [TestCleanup]
        public void CleanUp()
        {
            appCancellationToken.Cancel();
        }

        // PACT VERIFICATION
        [TestMethod]
        public void GetSomething_WhenTheTesterSomethingExists_ReturnsTheSomething()
        {

            var config = new PactVerifierConfig()
            {
                ProviderVersion = "1.0",
                PublishVerificationResults = true,
                Verbose = true
            };

            //Act / Assert 
            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier
                .ProviderState($"{serviceUri}/provider-states")
                .ServiceProvider("Todos API", serviceUri)
                .HonoursPactWith("Consumer")
                .PactUri(@".\pacts\consumer-todos_api.json")
                //or
                //.PactUri("https://broker.pact.dius.com.au/pacts/provider/Todos%20API/consumer/Consumer/latest", new PactUriOptions("broker_token"))
                .Verify();

        }
    }
}
