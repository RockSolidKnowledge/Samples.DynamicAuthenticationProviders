using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace Core
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMvc();

            services.AddAuthentication("cookie")
                .AddCookie("cookie");

            services.AddDynamicProviders() // Component setup
                .AddJsonStore(options => options.Path = "schemes.json") // Basic JSON store for auth schemes
                .AddOpenIdConnect() // Add OIDC support
                .AddSaml(); // Add SAML support
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
