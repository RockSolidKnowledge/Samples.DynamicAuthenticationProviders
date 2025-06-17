using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Logging;

namespace Core
{
    public static class HostingExtensions
    {
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
                .AddJsonStore(options => options.Path = "schemes.json") // Basic JSON store for auth schemes
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
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseDeveloperExceptionPage();

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
    }
}
