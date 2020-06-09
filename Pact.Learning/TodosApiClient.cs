using System;
using System.Net;
using System.Net.Http;

using Newtonsoft.Json;

using Pact.Models;

namespace Pact.Learning.Client
{
    public class TodosApiClient
    {
        private readonly HttpClient _client;

        public TodosApiClient(string baseUri = null)
        {
            _client = new HttpClient { BaseAddress = new Uri(baseUri ?? "http://jsonplaceholder.typicode.com") };
        }

        public Todo GetTodo(string id)
        {
            string reasonPhrase;

            var request = new HttpRequestMessage(HttpMethod.Get, "/todos/" + id);
            request.Headers.Add("Accept", "application/json");

            var response = _client.SendAsync(request);

            var content = response.Result.Content.ReadAsStringAsync().Result;
            var status = response.Result.StatusCode;

            reasonPhrase = response.Result.ReasonPhrase; //NOTE: any Pact mock provider errors will be returned here and in the response body

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                return !String.IsNullOrEmpty(content) ?
                  JsonConvert.DeserializeObject<Todo>(content)
                  : null;
            }

            throw new Exception(reasonPhrase);
        }
    }
}
