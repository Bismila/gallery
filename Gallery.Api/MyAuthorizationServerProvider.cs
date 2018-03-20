using Gallery.BAL.Interfaces;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Gallery.Api.Models;
using Microsoft.AspNet.Identity;



namespace Gallery.Api
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            if (context.UserName != null && context.Password != null)
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim("password", context.Password));
                context.Validated(identity);
            }
            else
            {
                context.SetError("invalid_grand", "Provided username and password is incorrect");
                return;
            }
        }
    }
}