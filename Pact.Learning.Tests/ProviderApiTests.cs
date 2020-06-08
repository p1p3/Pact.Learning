using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pact.Learning.Pacts;
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
        //private readonly app 
        private readonly CancellationTokenSource appCancellationToken = new CancellationTokenSource();
        const string serviceUri = "http://localhost:9222";
        public ProviderApiTests()
        {
            this._factory = new WebApplicationFactory<Pact.Provider.Startup>();



            var webHost = WebHost
                      .CreateDefaultBuilder(null)
                      .UseStartup<Pact.Provider.Startup>()
                      //.Configure(app => app.UseMiddleware<ProviderStateMiddleware>())
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

        [TestMethod]
        public void GetSomething_WhenTheTesterSomethingExists_ReturnsTheSomething()
        {


            var config = new PactVerifierConfig
            {
                //CustomHeaders = new Dictionary<string, string> { { "Authorization", "Basic VGVzdA==" } }, //This allows the user to set request headers that will be sent with every request the verifier sends to the provider
                Verbose = true //Output verbose verification logs to the test output
            };



            //using (var app =_factory.WithWebHostBuilder(builder => { builder.UseUrls(serviceUri); })){}

            //Act / Assert 
            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier
                .ProviderState($"{serviceUri}/provider-states")
                .ServiceProvider("Todos API", serviceUri)
                .HonoursPactWith("Consumer")
                .PactUri(@"C:\Users\P1p3\source\repos\Pact.Learning\Pact.Learning.Tests\pacts\consumer-todos_api.json")
               //or
               //.PactUri("http://pact-broker/pacts/provider/Something%20Api/consumer/Consumer/latest") //You can specify a http or https uri
               //or
               //.PactUri("http://pact-broker/pacts/provider/Something%20Api/consumer/Consumer/latest", new PactUriOptions("someuser", "somepassword")) //You can also specify http/https basic auth details                                                                                               //or
               //.PactUri("http://pact-broker/pacts/provider/Something%20Api/consumer/Consumer/latest", new PactUriOptions("sometoken")) //Or a bearer token                                                                                                                                          //or (if you're using the Pact Broker, you can use the various different features, including pending pacts)
               //.PactBroker("http://pact-broker", uriOptions: new PactUriOptions("sometoken"), enablePending: true, consumerVersionTags: new List<string> { "master" }, providerVersionTags: new List<string> { "master" }, consumerVersionSelectors: new List<VersionTagSelector> { new VersionTagSelector("master", false, true) })
               .Verify();

        }
    }
}
