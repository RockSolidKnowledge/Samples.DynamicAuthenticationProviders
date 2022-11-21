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
                    options.Licensee = "";
                    options.LicenseKey = "";

                })
                .AddJsonStore(options => options.Path = "schemes.json")
                .AddOpenIdConnect()
                // .AddSaml(o =>
                // {
                //     o.Licensee = "";
                //     o.LicenseKey = "";
                // })
                ;
            
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseDeveloperExceptionPage();

            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseIdentityServer();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}