using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Pact.Learning.Tests.Middlewares
{

    public class PactStartUpFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return builder =>
            {
                next(builder);
                builder.UseMiddleware<ProviderStateMiddleware>();
            };
        }
    }
}
