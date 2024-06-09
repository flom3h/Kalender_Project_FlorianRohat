using Serilog;
using Serilog.Core;

namespace Kalender_Project_FlorianRohat
{
    public class Log
    {
        public static Logger log = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7, fileSizeLimitBytes: 100_000_000)
            .CreateLogger();
    }
}
