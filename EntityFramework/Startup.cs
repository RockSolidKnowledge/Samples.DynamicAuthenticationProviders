using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Rsk.DynamicAuthenticationProviders.EntityFramework.Stores;
using Rsk.DynamicAuthenticationProviders.Stores;

namespace EntityFramework
{
    public class Startup
    {
        private const string ConnectionString = "test";

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMvc();

            services.AddAuthentication("cookie")
                .AddCookie("cookie");

            services.AddDynamicProviders(options =>
                {
                    // Component setup
                    options.Licensee = "";
                    options.LicenseKey = "";
                }) 
                .AddEntityFrameworkStore(options => options.UseInMemoryDatabase(ConnectionString))
                .AddOpenIdConnect() // Add OIDC support
                .AddSaml(); // Add SAML support
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            Seed(app.ApplicationServices);

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }

        public void Seed(IServiceProvider services)
        {
            var store = services.GetRequiredService<IAuthenticationSchemeStore>();

            if (store is AuthenticationSchemeStore)
            {
                store.AddScheme(new DynamicAuthenticationScheme(
                    "openid-1",
                    "OpenID",
                    typeof(OpenIdConnectHandler),
                    new OpenIdConnectOptions
                    {
                        Authority = "https://demo.identityserver.com",
                        ClientId = "dynamicauth-quickstart",
                        ResponseType = "id_token token",
                        Scope = {"openid", "profile", "api1"},
                        CallbackPath = "/signin/dynamic/openid-1",
                        SignInScheme = "cookie"
                    }));
            }
        }
    }
}
