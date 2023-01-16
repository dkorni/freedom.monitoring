using Serilog;
using Serilog.Core;

namespace Freedom.ServerMonitor.Extensions;

public static class LoggingExtensions
{
    public static WebApplicationBuilder AddLogger(this WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        
        Log.Logger = logger;
        builder.Host.UseSerilog();
        return builder;
    }
}