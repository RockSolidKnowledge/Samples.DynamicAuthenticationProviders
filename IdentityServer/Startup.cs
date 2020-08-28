using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    options.EmitStaticAudienceClaim = true;
                })
                .AddTestUsers(TestUsers.Users)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddDeveloperSigningCredential();

            var builder = services.AddDynamicProviders(options =>
                {
                    // Component setup
                    options.Licensee = "DEMO";
                    options.LicenseKey = "<your license key>";
                })
                .AddJsonStore(options => options.Path = "schemes.json")
                .AddOpenIdConnect();

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
            
            app.UseIdentityServer();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}