using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using System;

namespace JM.BlzrUrlList.Web
{
    public class ApiAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public ApiAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation, UrlApiConfig apiConfig)
            : base(provider, navigation)
        {
             
                ConfigureHandler(
                  authorizedUrls: new[] { apiConfig.EndPoint },
                  scopes: new[] { apiConfig.Scopes.Read, apiConfig.Scopes.Write });
          
           
        }
    }
}