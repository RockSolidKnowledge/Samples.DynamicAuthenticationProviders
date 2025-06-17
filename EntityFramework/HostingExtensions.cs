using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Rsk.DynamicAuthenticationProviders.EntityFramework.Stores;
using Rsk.DynamicAuthenticationProviders.Stores;

namespace EntityFramework
{
    public static class HostingExtensions
    {
        private const string ConnectionString = "test";

        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)

        {
            IdentityModelEventSource.ShowPII = true;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            builder.Services.AddMvc();
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication("cookie")
                .AddCookie("cookie");

            builder.Services.AddDynamicProviders(options =>
                {
                    // Component setup
                    options.Licensee = "";
                    options.LicenseKey = "";
                })
                .AddEntityFrameworkStore(options => options.UseInMemoryDatabase(ConnectionString))
                .AddOpenIdConnect(); // Add OIDC support

            /*builder.AddSaml(optionsAugmentor => // Add SAML support
            {
                optionsAugmentor.Licensee = "DEMO";
                optionsAugmentor.LicenseKey = "<your license key>";
            });*/

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseDeveloperExceptionPage();

            Seed(app.Services);

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // Map routes
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            return app;
        }

        public static void Seed(IServiceProvider services)
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
                        Scope = { "openid", "profile", "api1" },
                        CallbackPath = "/signin/dynamic/openid-1",
                        SignInScheme = "cookie"
                    }));
            }
        }
    }
}
