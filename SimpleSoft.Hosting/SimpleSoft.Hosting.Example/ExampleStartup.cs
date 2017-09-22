using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using SimpleSoft.Hosting.Params;

namespace SimpleSoft.Hosting.Example
{
    public class ExampleStartup : HostStartup
    {
        private readonly string[] _args;

        public ExampleStartup(string[] args)
        {
            _args = args;
        }

        public override void ConfigureConfigurationBuilder(IConfigurationBuilderParam param)
        {
            param.Builder
                .SetBasePath(param.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{param.Environment.Name}.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(_args);
        }

        public override void ConfigureConfiguration(IConfigurationHandlerParam param)
        {
            if (param.Environment.IsDevelopment())
                param.Configuration["CacheTimeoutInMs"] = "1000";
        }

        public override void ConfigureLoggerFactory(ILoggerFactoryHandlerParam param)
        {
            param.LoggerFactory.AddNLog();

            param.LoggerFactory.ConfigureNLog(
                param.Environment.ContentRootFileProvider.GetFileInfo(
                        param.Environment.IsDevelopment()
                            ? "nlog.config"
                            : $"nlog.{param.Environment.Name}.config")
                    .PhysicalPath);
        }

        public override void ConfigureServiceCollection(IServiceCollectionHandlerParam param)
        {
            param.ServiceCollection
                .AddOptions()
                .Configure<ExampleHostOptions>(param.Configuration)
                .AddSingleton(k => k.GetRequiredService<IOptions<ExampleHostOptions>>().Value);
        }

        public override IServiceProvider BuildServiceProvider(IServiceProviderBuilderParam param)
        {
            var container = new Autofac.ContainerBuilder();
            container.Populate(param.ServiceCollection);
            return new AutofacServiceProvider(container.Build());
        }
    }
}