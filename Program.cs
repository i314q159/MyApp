
using Serilog;

namespace MyApp;

class Program
{
    static void Main()
    {
        Log.Logger = new LoggerConfiguration()
        .WriteTo.Console()
        .WriteTo.File("log/a.log")
        .CreateLogger();

        Log.Information("Hello, Serilog!");
    }
}
