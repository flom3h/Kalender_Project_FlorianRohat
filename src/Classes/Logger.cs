using Serilog;
using Serilog.Core;
namespace Kalender_Project_FlorianRohat;

public static class Logger
{
    public static ILogger CreateLogger()
    {
        return new LoggerConfiguration()
            .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
            .CreateLogger();
    }
}