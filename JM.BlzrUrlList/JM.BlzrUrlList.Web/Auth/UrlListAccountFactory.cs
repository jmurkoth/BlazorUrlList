

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Logging;

public class UrlListAccountFactory: AccountClaimsPrincipalFactory<UrlListUserAccount>
{
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<UrlListAccountFactory> logger;

    public UrlListAccountFactory(IAccessTokenProviderAccessor accessor,
        IServiceProvider serviceProvider,
        ILogger<UrlListAccountFactory> logger)
        : base(accessor)
    {
        this.serviceProvider = serviceProvider;
        this.logger = logger;
    }
     public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
        UrlListUserAccount account,
        RemoteAuthenticationUserOptions options)
        {
              var initialUser = await base.CreateUserAsync(account, options);
              
                if (initialUser.Identity.IsAuthenticated)
                {
                    var userIdentity = (ClaimsIdentity)initialUser.Identity;
                    foreach (var role in account.Roles)
                    {
                        userIdentity.AddClaim(new Claim("appRole", role));
                    }

                    foreach (var wid in account.Wids)
                    {
                        userIdentity.AddClaim(new Claim("directoryRole", wid));
                    }
                }
                return initialUser;
        }
}