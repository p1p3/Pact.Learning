using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pact.Learning.Pacts;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using System.Collections.Generic;

namespace Pact.Learning.Tests
{
    [TestClass]
    public class ConsumerTodosApiTests
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;
        private ConsumerTodosApiPact pact;
        public ConsumerTodosApiTests()
        {
            this.pact = new ConsumerTodosApiPact();
            _mockProviderService = pact.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = pact.MockProviderServiceBaseUri;
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            this.pact.Dispose();
        }

        [TestMethod]
        public void GetSomething_WhenTheTesterSomethingExists_ReturnsTheSomething()
        {
            //Arrange
            _mockProviderService
              .Given("There is a something with id '1'")
              .UponReceiving("A GET request to retrieve the todo")
              .With(new ProviderServiceRequest
              {
                  Method = HttpVerb.Get,
                  Path = "/todos/1",
                  Headers = new Dictionary<string, object>
                {
          { "Accept", "application/json" }
                }
              })
              .WillRespondWith(new ProviderServiceResponse
              {
                  Status = 200,
                  Headers = new Dictionary<string, object>
                {
          { "Content-Type", "application/json; charset=utf-8" }
                },
                  Body = new //NOTE: Note the case sensitivity here, the body will be serialised as per the casing defined
                  {
                      id = "1",
                      userId = "1",
                      title = "delectus aut autem",
                      completed = false
                  }
              }); //NOTE: WillRespondWith call must come last as it will register the interaction

            var consumer = new TodosApiClient(_mockProviderServiceBaseUri);

            //Act
            var result = consumer.GetTodo("1");

            //Assert
            Assert.AreEqual("1", result.Id);

            _mockProviderService.VerifyInteractions(); //NOTE: Verifies that interactions registered on the mock provider are called at least once
        }
    }
}
