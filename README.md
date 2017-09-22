# Hosting
Library to simplify the hosting of console applications by making it easier to setup dependency injection, logging and configurations.

## Installation
The library is available via [NuGet](https://www.nuget.org/packages?q=SimpleSoft.Hosting) packages:

| NuGet | Description | Version |
| --- | --- | --- |
| [SimpleSoft.Hosting.Abstractions](https://www.nuget.org/packages/simplesoft.hosting.abstractions) | interfaces and abstract implementations (`IHost`, `IHostBuilder`, ...) | [![NuGet](https://img.shields.io/nuget/vpre/simplesoft.hosting.abstractions.svg)](https://www.nuget.org/packages/simplesoft.hosting.abstractions) |
| [SimpleSoft.Hosting](https://www.nuget.org/packages/simplesoft.hosting) | library implementation that typically is only known by the main project | [![NuGet](https://img.shields.io/nuget/vpre/simplesoft.hosting.svg)](https://www.nuget.org/packages/simplesoft.hosting) |

### Package Manager
```powershell
Install-Package SimpleSoft.Hosting.Abstractions
Install-Package SimpleSoft.Hosting
```

### .NET CLI
```powershell
dotnet add package SimpleSoft.Hosting.Abstractions
dotnet add package SimpleSoft.Hosting
```
## Compatibility
This library is compatible with the following frameworks:

* SimpleSoft.Hosting.Abstractions
  * .NET Standard 1.1;
* SimpleSoft.Hosting
  * .NET Framework 4.5.1;
  * .NET Standard 1.3;
  * .NET Standard 1.5;

## Usage
Documentation is available via [wiki](https://github.com/simplesoft-pt/Hosting/wiki) or you can check the [working examples](https://github.com/simplesoft-pt/Hosting/tree/master/SimpleSoft.Hosting/SimpleSoft.Hosting.Example) or [test](https://github.com/simplesoft-pt/Hosting/tree/master/test) code.

Here is an example of a console application:
```csharp
public class Program
{
    private static readonly CancellationTokenSource TokenSource;

    static Program()
    {
        TokenSource = new CancellationTokenSource();
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            TokenSource.Cancel();
            eventArgs.Cancel = true;
        };
    }

    public static void Main(string[] args) =>
        MainAsync(args, TokenSource.Token).ConfigureAwait(false).GetAwaiter().GetResult();

    private static async Task MainAsync(string[] args, CancellationToken ct)
    {
        var loggerFactory = new LoggerFactory()
            .AddConsole(LogLevel.Trace, true);

        var logger = loggerFactory.CreateLogger<Program>();

        logger.LogInformation("Application started");
        try
        {
            using (var hostBuilder = new HostBuilder("ASPNETCORE_ENVIRONMENT")
                .UseLoggerFactory(loggerFactory)
                .UseStartup<Startup>()
                .ConfigureConfigurationBuilder(p => p.Builder.AddCommandLine(args)))
            {
                await hostBuilder.RunHostAsync<Host>(ct);
            }
        }
        catch (TaskCanceledException)
        {
            logger.LogWarning("Application was terminated by user request");
        }
        catch (Exception e)
        {
            logger.LogCritical(0, e, "Unexpected exception");
        }
        finally
        {
            logger.LogInformation("Application terminated. Press <enter> to exit...");
            Console.ReadLine();
        }
    }

    private class Startup : HostStartup
    {
        public override void ConfigureConfigurationBuilder(IConfigurationBuilderParam param)
        {
            param.Builder
                .SetBasePath(param.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{param.Environment.Name}.json", true, true)
                .AddEnvironmentVariables();
        }

        public override IServiceProvider BuildServiceProvider(IServiceProviderBuilderParam param)
        {
            var container = new Autofac.ContainerBuilder();
            container.Populate(param.ServiceCollection);
            return new AutofacServiceProvider(container.Build());
        }
    }

    private class Host : IHost
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _configurationRoot;
        private readonly ILogger<Host> _logger;

        public Host(IHostingEnvironment env, IConfigurationRoot configurationRoot, ILogger<Host> logger)
        {
            _env = env;
            _configurationRoot = configurationRoot;
            _logger = logger;
        }

        public Task RunAsync(CancellationToken ct)
        {
            _logger.LogDebug("Running host...");

            return Task.CompletedTask;
        }
    }
}
```
