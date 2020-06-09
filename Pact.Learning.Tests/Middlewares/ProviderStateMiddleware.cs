using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pact.Learning.Tests.Middlewares
{
    public class ProviderState
    {
        public string Consumer { get; set; }
        public string State { get; set; }
    }

    public class ProviderStateMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ConsumerName = "Event API Consumer";
        private readonly Func<IDictionary<string, object>, Task> m_next;
        private readonly IDictionary<string, Action> _providerStates;


        public ProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;
            _providerStates = new Dictionary<string, Action>
        {
            {
                "There is a something with id '1'",
                AddTesterIfItDoesntExist
            }
        };
        }

        private void AddTesterIfItDoesntExist()
        {
            // CREATE THE TODO with 1
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Value == "/provider-states")
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                if (context.Request.Method == HttpMethod.Post.ToString() &&
                    context.Request.Body != null)
                {
                    string jsonRequestBody;
                    using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                    {
                        jsonRequestBody = await reader.ReadToEndAsync();
                    }

                    var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                    //A null or empty provider state key must be handled
                    if (providerState != null &&
                        !string.IsNullOrEmpty(providerState.State) &&
                        providerState.Consumer == ConsumerName)
                    {
                        _providerStates[providerState.State].Invoke();
                    }

                    await context.Response.WriteAsync(string.Empty);
                }
                else
                {
                    // Call the next delegate/middleware in the pipeline
                    await _next(context);
                }
            }
        }
    }
}
