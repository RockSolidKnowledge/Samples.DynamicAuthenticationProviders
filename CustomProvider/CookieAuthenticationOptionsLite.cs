using Microsoft.AspNetCore.Authentication.Cookies;
using Rsk.DynamicAuthenticationProviders.Serializers.Entities;

namespace CustomProvider
{
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

        public bool? SlidingExpiration { get; init; }
        public PathString? LoginPath { get; init; }
        public PathString? LogoutPath { get; init; }
        public PathString? AccessDeniedPath { get; init; }
        public string ReturnUrlParameter { get; init; } = string.Empty;
        public TimeSpan? ExpireTimeSpan { get; init; }

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
