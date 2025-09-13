namespace TradingPlatform.Utils
{
    using Microsoft.Extensions.Configuration;

    public static class AppConfig
    {
        public static IConfiguration? Configuration { get; private set; }

        public static void Init(IConfiguration config)
        {
            Configuration = config;
        }

        public static string? Get(string key)
        {
            return Configuration?[key];
        }
    }
}
