using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;

namespace CustomProvider
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

            builder.Services.AddAuthenticationCore();

            builder.Services.TryAddSingleton<ISystemClock, SystemClock>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<
                IPostConfigureOptions<CookieAuthenticationOptions>,
                PostConfigureCookieAuthenticationOptions>());

            builder.Services.AddDynamicProviders(options =>
                {
                    // Component setup
                    options.Licensee = "";
                    options.LicenseKey = "";

                    // Custom authentiation provider
                    options.HandlerMap.Add(typeof(CookieAuthenticationHandler), typeof(CookieAuthenticationOptions));

                    // Optional custom JSON serialization
                    options.SerializerSettings.Converters.Add(new CookieAuthenticationOptionsConverter());
                })
                .AddJsonStore(options => options.Path = "schemes.json"); // Basic JSON store for auth schemes

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
