using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using web_automatic_routing.Configuration;

namespace automatic_routing.Middleware
{
    public class FeatureRoutingMiddleware
    {

        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly EndpointDataSource _endpointDataSource;
        private readonly FeatureRoutingConfiguration _routingConfiguration;
        public FeatureRoutingMiddleware(
          ILogger<FeatureRoutingMiddleware> logger,
          EndpointDataSource endpointDataSource,
          FeatureRoutingConfiguration routingConfiguration,
          RequestDelegate next)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _endpointDataSource = endpointDataSource ?? throw new ArgumentNullException(nameof(endpointDataSource));
            _routingConfiguration = routingConfiguration ?? throw new ArgumentNullException(nameof(routingConfiguration));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public Task Invoke(HttpContext httpContext)
        {

            if (_endpointDataSource.Endpoints?.Count > 0)
            {
                var endpointMatchResult = _routingConfiguration.Match(httpContext, _endpointDataSource.Endpoints);

                if(endpointMatchResult.Redirect)
                    httpContext.SetEndpoint(endpointMatchResult.RedirectEndpoint);
            }
            
            return _next(httpContext);
        }
    }
}
