using Serilog;

namespace API.Configuration
{
    public static class LogConfiguration
    {
        public static void ConfigureSerilog(this WebApplicationBuilder builder)
        {
            // Set up logs for server
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
