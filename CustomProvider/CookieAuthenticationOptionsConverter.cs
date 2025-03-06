using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;

namespace CustomProvider
{
    public class CookieAuthenticationOptionsConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => typeof(CookieAuthenticationOptions) == objectType;

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is CookieAuthenticationOptions options)
            {
                var optionsLite = new CookieAuthenticationOptionsLite(options);
                serializer.Serialize(writer, optionsLite);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var optionsLite = serializer.Deserialize<CookieAuthenticationOptionsLite>(reader);
            return optionsLite?.ToOptions() ?? new CookieAuthenticationOptions();
        }
    }
}
