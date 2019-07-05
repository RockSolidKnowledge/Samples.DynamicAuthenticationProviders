using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Rsk.DynamicAuthenticationProviders.Serializers.Entities;

namespace CustomProvider
{
    public class Startup
    {
        private const string ConnectionString = "test";

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddMvc();

            services.AddAuthenticationCore();

            services.TryAddSingleton<ISystemClock, SystemClock>();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<
                IPostConfigureOptions<CookieAuthenticationOptions>, 
                PostConfigureCookieAuthenticationOptions>());

            services.AddDynamicProviders(options =>
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
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }

    public class CookieAuthenticationOptionsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(CookieAuthenticationOptions) == objectType;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var optionsLite = new CookieAuthenticationOptionsLite(value as CookieAuthenticationOptions);
            serializer.Serialize(writer, optionsLite);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var optionsLite = serializer.Deserialize<CookieAuthenticationOptionsLite>(reader);
            return optionsLite.ToOptions();
        }
    }

    public class CookieAuthenticationOptionsLite : AuthenticationSchemeOptionsLite
    {
        public CookieAuthenticationOptionsLite() { }

        public CookieAuthenticationOptionsLite(CookieAuthenticationOptions options) : base(options)
        {
            SlidingExpiration = options.SlidingExpiration;
            LoginPath = options.LoginPath;
            LogoutPath = options.LogoutPath;
            AccessDeniedPath = options.AccessDeniedPath;
            ReturnUrlParameter = options.ReturnUrlParameter;
            ExpireTimeSpan = options.ExpireTimeSpan;
        }

        public bool? SlidingExpiration { get; set; }
        public PathString? LoginPath { get; set; }
        public PathString? LogoutPath { get; set; }
        public PathString? AccessDeniedPath { get; set; }
        public string ReturnUrlParameter { get; set; }
        public TimeSpan? ExpireTimeSpan { get; set; }

        public CookieAuthenticationOptions ToOptions()
        {
            var options = new CookieAuthenticationOptions();
            base.ToOptions(options);

            options.SlidingExpiration = SlidingExpiration ?? options.SlidingExpiration;
            options.LoginPath = LoginPath ?? options.LoginPath;
            options.LogoutPath = LogoutPath ?? options.LogoutPath;
            options.AccessDeniedPath = AccessDeniedPath ?? options.AccessDeniedPath;
            options.ReturnUrlParameter = ReturnUrlParameter ?? options.ReturnUrlParameter;
            options.ExpireTimeSpan = ExpireTimeSpan ?? options.ExpireTimeSpan;

            return options;
        }
    }
}
