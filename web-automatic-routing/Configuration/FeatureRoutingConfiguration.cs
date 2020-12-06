using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_automatic_routing.Configuration
{
    public class FeatureRoutingConfiguration
    {
        public List<FeatureRouting> Features { get; set; } = new List<FeatureRouting>();


        public (bool Redirect, Endpoint RedirectEndpoint) Match(HttpContext currentContext, IReadOnlyList<Endpoint> registeredEndpoints)
        {

            bool redirect = false;
            Endpoint redirectEndpoint = currentContext.GetEndpoint();

            if (redirectEndpoint != null)
            {
                List<FeatureRouting> matchesHeader = MatchAgainsHeaderConfiguration(currentContext);

                if (matchesHeader?.Count > 0)
                {
                    var currentEndpointControllerMetadata = redirectEndpoint.Metadata.FirstOrDefault(b => b is ControllerActionDescriptor) as ControllerActionDescriptor;

                    foreach (var featureMatch in matchesHeader)
                    {
                        if (featureMatch.Action.From.Equals(currentEndpointControllerMetadata))
                        {
                            var toEnpointMatch = registeredEndpoints.FirstOrDefault(b => b.Metadata.OfType<ControllerActionDescriptor>().FirstOrDefault(c => featureMatch.Action.To.Equals(c)) != null);
                            if (toEnpointMatch != null)
                            {
                                redirectEndpoint = toEnpointMatch;
                                redirect = true;
                                break;
                            }
                        }
                    }
                } 
            }

            return (redirect, redirectEndpoint);
        }

        private List<FeatureRouting> MatchAgainsHeaderConfiguration(HttpContext currentContext)
        {
            List<FeatureRouting> matchedFeatures = new List<FeatureRouting>();

            Features.ForEach(c =>
            {
                var featureHeader = currentContext.Request.Headers?.FirstOrDefault(b => b.Key.ToString().Equals(c.Header.Key, StringComparison.InvariantCultureIgnoreCase) &&
                                                                                        b.Value.ToString().Equals(c.Header.Value, StringComparison.InvariantCultureIgnoreCase));
                if (featureHeader?.Key != null)
                {
                    matchedFeatures.Add(c);
                }
            });

            return matchedFeatures;
        }

        public class FeatureRouting
        {
            public Header Header { get; set; }
            public RoutingAction Action { get; set; }

        }

        public class Header
        {
            public string Key { get; set; }
            public string Value { get; set; }
        }

        public class RoutingAction
        {
            public EndpointInfo From { get; set; }
            public EndpointInfo To { get; set; }
        }

        public class EndpointInfo
        {
            public string Controller { get; set; }
            public string Action { get; set; }

            public bool Equals(ControllerActionDescriptor controllerActionDescriptor)
            {
                if (controllerActionDescriptor == null)
                    return false;

                return Controller.Equals(controllerActionDescriptor?.ControllerName, StringComparison.InvariantCultureIgnoreCase) &&
                    Action.Equals(controllerActionDescriptor?.ActionName, StringComparison.InvariantCultureIgnoreCase);

            }
        }

    }
}
