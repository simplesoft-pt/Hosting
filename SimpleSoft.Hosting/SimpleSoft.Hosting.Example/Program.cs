using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;

namespace SimpleSoft.Hosting.Example
{
    public class Program
    {
        private static readonly CancellationTokenSource TokenSource;

        static Program()
        {
            TokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, args) =>
            {
                TokenSource.Cancel();
                args.Cancel = true;
            };
        }

        public static void Main(string[] args) =>
            MainAsync(args, TokenSource.Token).ConfigureAwait(false).GetAwaiter().GetResult();

        public static async Task MainAsync(string[] args, CancellationToken ct)
        {
            await HostingUsingOnlyInterfaceMethodsAsync(args, ct);
            await HostingExtensionMethodsAsync(args, ct);
            await HostingUsingStartupAsync(args, ct);
        }

        private static async Task HostingUsingOnlyInterfaceMethodsAsync(string[] args, CancellationToken ct)
        {
            var loggerFactory = new LoggerFactory()
                .AddConsole(LogLevel.Trace, true);

            var logger = loggerFactory.CreateLogger<Program>();

            using (logger.BeginScope("Example:{exampleNumber}", "01"))
            {
                logger.LogInformation("Hosting an application using only interface methods");

                using (var hostBuilder = new HostBuilder("ASPNETCORE_ENVIRONMENT"))
                {
                    hostBuilder.SetLoggerFactory(loggerFactory);
                    hostBuilder.AddConfigurationBuilderHandler(param =>
                    {
                        param.Builder
                            .SetBasePath(param.Environment.ContentRootPath)
                            .AddJsonFile("appsettings.json", true, true)
                            .AddJsonFile($"appsettings.{param.Environment.Name}.json", true, true)
                            .AddEnvironmentVariables()
                            .AddCommandLine(args);
                    });
                    hostBuilder.AddConfigurationHandler(param =>
                    {
                        if (param.Environment.IsDevelopment())
                            param.Configuration["CacheTimeoutInMs"] = "1000";
                    });
                    hostBuilder.AddLoggerFactoryHandler(param =>
                    {
                        param.LoggerFactory.AddNLog();

                        param.LoggerFactory.ConfigureNLog(
                            param.Environment.ContentRootFileProvider.GetFileInfo(
                                    param.Environment.IsDevelopment()
                                        ? "nlog.config"
                                        : $"nlog.{param.Environment.Name}.config")
                                .PhysicalPath);
                    });
                    hostBuilder.AddServiceCollectionHandler(param =>
                    {
                        param.ServiceCollection
                            .AddOptions()
                            .Configure<ExampleHostOptions>(param.Configuration)
                            .AddSingleton(k => k.GetRequiredService<IOptions<ExampleHostOptions>>().Value);
                    });
                    hostBuilder.ServiceProviderBuilder = param =>
                    {
                        var container = new Autofac.ContainerBuilder();
                        container.Populate(param.ServiceCollection);
                        return new AutofacServiceProvider(container.Build());
                    };

                    using (var ctx = hostBuilder.BuildRunContext<ExampleHost>())
                    {
                        await ctx.Host.RunAsync(ct);
                    }
                }

                logger.LogInformation("Terminated");
            }
        }

        private static async Task HostingExtensionMethodsAsync(string[] args, CancellationToken ct)
        {
            var loggerFactory = new LoggerFactory()
                .AddConsole(LogLevel.Trace, true);

            var logger = loggerFactory.CreateLogger<Program>();

            using (logger.BeginScope("Example:{exampleNumber}", "02"))
            {
                logger.LogInformation("Hosting an application using extension methods");

                using (var hostBuilder = new HostBuilder("ASPNETCORE_ENVIRONMENT")
                    .UseLoggerFactory(loggerFactory)
                    .ConfigureConfigurationBuilder(param =>
                    {
                        param.Builder
                            .SetBasePath(param.Environment.ContentRootPath)
                            .AddJsonFile("appsettings.json", true, true)
                            .AddJsonFile($"appsettings.{param.Environment.Name}.json", true, true)
                            .AddEnvironmentVariables()
                            .AddCommandLine(args);
                    })
                    .ConfigureConfiguration(param =>
                    {
                        if (param.Environment.IsDevelopment())
                            param.Configuration["CacheTimeoutInMs"] = "1000";
                    })
                    .ConfigureLoggerFactory(param =>
                    {
                        param.LoggerFactory.AddNLog();

                        param.LoggerFactory.ConfigureNLog(
                            param.Environment.ContentRootFileProvider.GetFileInfo(
                                    param.Environment.IsDevelopment()
                                        ? "nlog.config"
                                        : $"nlog.{param.Environment.Name}.config")
                                .PhysicalPath);
                    })
                    .ConfigureServiceCollection(param =>
                    {
                        param.ServiceCollection
                            .AddOptions()
                            .Configure<ExampleHostOptions>(param.Configuration)
                            .AddSingleton(k => k.GetRequiredService<IOptions<ExampleHostOptions>>().Value);
                    })
                    .UseServiceProviderBuilder(param =>
                    {
                        var container = new Autofac.ContainerBuilder();
                        container.Populate(param.ServiceCollection);
                        return new AutofacServiceProvider(container.Build());
                    }))
                {
                    await hostBuilder.RunHostAsync<ExampleHost>(ct);
                }

                logger.LogInformation("Terminated");
            }
        }

        private static async Task HostingUsingStartupAsync(string[] args, CancellationToken ct)
        {
            var loggerFactory = new LoggerFactory()
                .AddConsole(LogLevel.Trace, true);

            var logger = loggerFactory.CreateLogger<Program>();

            using (logger.BeginScope("Example:{exampleNumber}", "03"))
            {
                logger.LogInformation("Hosting an application using startup class");

                using (var hostBuilder = new HostBuilder("ASPNETCORE_ENVIRONMENT")
                    .UseLoggerFactory(loggerFactory)
                    .UseStartup(new ExampleStartup(args)))
                {
                    await hostBuilder.RunHostAsync<ExampleHost>(ct);
                }

                logger.LogInformation("Terminated");
            }
        }
    }
}
