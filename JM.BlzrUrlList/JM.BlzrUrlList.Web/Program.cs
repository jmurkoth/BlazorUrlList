using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JM.BlzrUrlList.Web.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace JM.BlzrUrlList.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.Services.AddSingleton<AppState>();
            builder.Services.AddTransient<ApiAuthorizationMessageHandler>();
            var apiConfig=new UrlApiConfig();
            builder.Configuration.Bind("UrlApi",apiConfig );
            var apiEndPoint = builder.Configuration.GetValue<string>("UrlApi");
            var apiScopes = builder.Configuration.GetValue<string>("UrlApiTokenScope");
             builder.Services.AddSingleton<UrlApiConfig>(apiConfig);
            // Add the HTTP Client
            builder.Services.AddHttpClient("webapi", client => client.BaseAddress = new Uri(apiConfig.EndPoint));
            // if we dont keep them separate we get a token exception for unauthenticated calls
            builder.Services.AddHttpClient("securewebapi", client => client.BaseAddress = new Uri(apiConfig.EndPoint))
                .AddHttpMessageHandler<ApiAuthorizationMessageHandler>();
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("webapi"));
             builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("securewebapi"));
   
            builder.Services.AddScoped<UrlService>();
            builder.Services.AddMsalAuthentication<RemoteAuthenticationState,UrlListUserAccount>(options =>
            {
                builder.Configuration.Bind("BlzrUrlAzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.LoginMode="redirect";
                options.UserOptions.RoleClaim = "appRole"; // without this authorize roles will not work even if claim is there
                options.ProviderOptions.DefaultAccessTokenScopes.Add(apiConfig.Scopes.Default);
            }).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, UrlListUserAccount, UrlListAccountFactory>();

            await builder.Build().RunAsync();
        }
    }
}
