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
            services.AddControllersWithViews();

            services.AddAuthentication("cookie")
                .AddCookie("cookie");

            var builder = services.AddDynamicProviders(options =>
                {
                    // Component setup
                    options.Licensee = "";
                    options.LicenseKey = "";
                })
                .AddJsonStore(options => options.Path = "schemes.json") // Basic JSON store for auth schemes
                .AddOpenIdConnect(); // Add OIDC support

            /*builder.AddSaml(optionsAugmentor => // Add SAML support
            {
                optionsAugmentor.Licensee = "DEMO";
                optionsAugmentor.LicenseKey = "<your license key>";
            });*/
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
