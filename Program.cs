using Serilog;

namespace MyApp;

class Program
{
    static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("log/log_.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

        try
        {
            Log.Information("Hello, Serilog!");
            // ... 其他业务代码
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
