namespace SiecaAPI.Commons
{
    public static class AppParamsTools
    {
        private static readonly IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

        public static string GetEnvironmentVariable(string nombreParam)
        {
            return configuration[nombreParam] ?? "";
        }
    }
}
