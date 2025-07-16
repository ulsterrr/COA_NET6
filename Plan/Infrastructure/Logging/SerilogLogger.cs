using Serilog;

namespace Infrastructure.Logging
{
    public static class SerilogLogger
    {
        public static ILogger CreateLogger()
        {
            return new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
