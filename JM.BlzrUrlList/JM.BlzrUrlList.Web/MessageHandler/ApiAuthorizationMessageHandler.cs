using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using System;

namespace JM.BlzrUrlList.Web
{
    public class ApiAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        private readonly IConfiguration configuration;

        public ApiAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation, IConfiguration configuration)
            : base(provider, navigation)
        {
                var apiEndPoint = configuration.GetValue<string>("UrlApi");
                var apiReadScope = configuration.GetValue<string>("UrlApiReadScope");
                var apiWriteScope = configuration.GetValue<string>("UrlApiWriteScope");
                ConfigureHandler(
                  authorizedUrls: new[] { apiEndPoint },
                  scopes: new[] { apiReadScope, apiWriteScope });
          
           
            this.configuration = configuration;
        }
    }
}