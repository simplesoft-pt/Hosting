#region License
// The MIT License (MIT)
// 
// Copyright (c) 2017 SimpleSoft.pt
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleSoft.Hosting.Params;

namespace SimpleSoft.Hosting
{
    /// <summary>
    /// Extensions for <see cref="IHostBuilder"/> instances.
    /// </summary>
    public static class HostBuilderExtensions
    {
        #region ConfigureConfigurationBuilder

        /// <summary>
        /// Adds an handler to the <see cref="IHostBuilder.ConfigurationBuilderHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureConfigurationBuilder<TBuilder>(this TBuilder builder, Action<IConfigurationBuilderParam> handler)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddConfigurationBuilderHandler(handler);
            return builder;
        }

        /// <summary>
        /// Adds an handler to the <see cref="IHostBuilder.ConfigurationBuilderHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureConfigurationBuilder<TBuilder>(this TBuilder builder, Action<IConfigurationBuilder, IHostingEnvironment> handler)
            where TBuilder : IHostBuilder
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            return builder.ConfigureConfigurationBuilder(p => handler(p.Builder, p.Environment));
        }

        #endregion

        #region ConfigureConfiguration

        /// <summary>
        /// Adds an handler to the <see cref="IHostBuilder.ConfigurationHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureConfiguration<TBuilder>(this TBuilder builder, Action<IConfigurationHandlerParam> handler)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddConfigurationHandler(handler);
            return builder;
        }

        /// <summary>
        /// Adds an handler to the <see cref="IHostBuilder.ConfigurationHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureConfiguration<TBuilder>(this TBuilder builder, Action<IConfigurationRoot, IHostingEnvironment> handler)
            where TBuilder : IHostBuilder
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            return builder.ConfigureConfiguration(p => handler(p.Configuration, p.Environment));
        }

        #endregion

        #region UseLoggerFactory

        /// <summary>
        /// Uses the given <see cref="ILoggerFactory"/> instance.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="loggerFactory">The logger factory to use</param>
        /// <param name="disposeFactory">Should the builder also dispose the factory when disposed?</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder UseLoggerFactory<TBuilder>(this TBuilder builder, ILoggerFactory loggerFactory, bool disposeFactory = true)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.SetLoggerFactory(loggerFactory, disposeFactory);
            return builder;
        }

        /// <summary>
        /// Adds an handler to the <see cref="IHostBuilder.LoggerFactoryHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureLoggerFactory<TBuilder>(this TBuilder builder, Action<ILoggerFactoryHandlerParam> handler)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddLoggerFactoryHandler(handler);
            return builder;
        }

        /// <summary>
        /// Adds an handler to the <see cref="IHostBuilder.LoggerFactoryHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureLoggerFactory<TBuilder>(this TBuilder builder, Action<ILoggerFactory, IConfiguration, IHostingEnvironment> handler)
            where TBuilder : IHostBuilder
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            
            return builder.ConfigureLoggerFactory(p => handler(p.LoggerFactory, p.Configuration, p.Environment));
        }

        #endregion

        #region ConfigureServiceCollection

        /// <summary>
        /// Adds an handler to the <see cref="IHostBuilder.ServiceCollectionHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureServiceCollection<TBuilder>(this TBuilder builder, Action<IServiceCollectionHandlerParam> handler)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddServiceCollectionHandler(handler);
            return builder;
        }

        /// <summary>
        /// Adds an handler to the <see cref="IHostBuilder.ServiceCollectionHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureServiceCollection<TBuilder>(this TBuilder builder, Action<IServiceCollection, ILoggerFactory, IConfiguration, IHostingEnvironment> handler)
            where TBuilder : IHostBuilder
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            return builder.ConfigureServiceCollection(p => handler(p.ServiceCollection, p.LoggerFactory, p.Configuration, p.Environment));
        }

        #endregion

        #region UseServiceProviderBuilder

        /// <summary>
        /// Uses the given function as the <see cref="IServiceProvider"/> builder.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="providerBuilder">The provider builder to use</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder UseServiceProviderBuilder<TBuilder>(this TBuilder builder, Func<IServiceProviderBuilderParam, IServiceProvider> providerBuilder)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ServiceProviderBuilder = providerBuilder;
            return builder;
        }

        /// <summary>
        /// Uses the given function as the <see cref="IServiceProvider"/> builder.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="providerBuilder">The provider builder to use</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder UseServiceProviderBuilder<TBuilder>(this TBuilder builder, Func<IServiceCollection, ILoggerFactory, IConfiguration, IHostingEnvironment, IServiceProvider> providerBuilder)
            where TBuilder : IHostBuilder
        {
            if (providerBuilder == null) throw new ArgumentNullException(nameof(providerBuilder));

            return builder.UseServiceProviderBuilder(p =>
                providerBuilder(p.ServiceCollection, p.LoggerFactory, p.Configuration, p.Environment));
        }

        #endregion

        #region Configure

        /// <summary>
        /// Adds an handler to the <see cref="IHostBuilder.ConfigureHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder Configure<TBuilder>(this TBuilder builder, Action<IConfigureHandlerParam> handler)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddConfigureHandler(handler);
            return builder;
        }

        /// <summary>
        /// Adds an handler to the <see cref="IHostBuilder.ConfigureHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder Configure<TBuilder>(this TBuilder builder, Action<IServiceProvider, ILoggerFactory, IConfiguration, IHostingEnvironment> handler)
            where TBuilder : IHostBuilder
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            
            return builder.Configure(p => handler(p.ServiceProvider, p.LoggerFactory, p.Configuration, p.Environment));
        }

        #endregion

        #region UseStartup

        /// <summary>
        /// Uses the given startup class to configure hosts.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="startup">The startup instance</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder UseStartup<TBuilder>(this TBuilder builder, IHostStartup startup)
            where TBuilder : class, IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (startup == null) throw new ArgumentNullException(nameof(startup));

            builder.AddConfigurationBuilderHandler(startup.ConfigureConfigurationBuilder);
            builder.AddConfigurationHandler(startup.ConfigureConfiguration);
            builder.AddLoggerFactoryHandler(startup.ConfigureLoggerFactory);
            builder.AddServiceCollectionHandler(startup.ConfigureServiceCollection);
            builder.ServiceProviderBuilder = startup.BuildServiceProvider;
            builder.AddConfigureHandler(startup.Configure);

            return builder;
        }

        /// <summary>
        /// Uses the given startup class to configure hosts.
        /// </summary>
        /// <typeparam name="TStartup">The startup type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IHostBuilder UseStartup<TStartup>(this IHostBuilder builder)
            where TStartup : class, IHostStartup, new()
        {
            builder.UseStartup(new TStartup());

            return builder;
        }

        #endregion

        #region RunHost

        /// <summary>
        /// Builds and runs a host instance of the given type.
        /// </summary>
        /// <typeparam name="THost">The host type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="ct">The cancellation</param>
        /// <returns>Task to be awaited</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task RunHostAsync<THost>(this IHostBuilder builder, CancellationToken ct = default (CancellationToken)) 
            where THost : class, IHost
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            using (var ctx = builder.BuildRunContext<THost>())
            {
                var logger = ctx.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<IHostBuilder>();
                using (logger.BeginScope("HostType:'{hostTypeName}' ExecutionId:{executionId}", typeof(THost).Name, ctx.Id))
                {
                    await ctx.Host.RunAsync(ct).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Builds and runs a host instance of the given type.
        /// </summary>
        /// <typeparam name="THost">The host type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void RunHost<THost>(this IHostBuilder builder) 
            where THost : class, IHost
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.RunHostAsync<THost>(CancellationToken.None).ConfigureAwait(false)
                .GetAwaiter().GetResult();
        }

        #endregion

        #region Run

        /// <summary>
        /// Builds a host for running the given handler.
        /// </summary>
        /// <param name="builder">The builder to use</param>
        /// <param name="runHandler">The run handler</param>
        /// <param name="ct">The cancellation</param>
        /// <returns>Task to be awaited</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task RunAsync(
            this IHostBuilder builder, Func<IServiceProvider, CancellationToken, Task> runHandler, CancellationToken ct = default(CancellationToken))
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (runHandler == null) throw new ArgumentNullException(nameof(runHandler));

            using (var ctx = builder.BuildRunContext<HandlerHost>())
            {
                var logger = ctx.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<IHostBuilder>();
                using (logger.BeginScope("HostType:'{hostTypeName}' ExecutionId:{executionId}", nameof(HandlerHost), ctx.Id))
                {
                    var serviceProvider = ctx.ServiceProvider;
                    ctx.Host.Handler = async c =>
                    {
                        await runHandler(serviceProvider, c).ConfigureAwait(false);
                    };
                    await ctx.Host.RunAsync(ct).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Builds and runs a host instance of the given type.
        /// </summary>
        /// <param name="builder">The builder to use</param>
        /// <param name="runHandler">The run handler</param>
        /// <param name="ct">The cancellation</param>
        /// <returns>Task to be awaited</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static async Task RunAsync(
            this IHostBuilder builder, Func<IServiceProvider, Task> runHandler, CancellationToken ct = default(CancellationToken))
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (runHandler == null) throw new ArgumentNullException(nameof(runHandler));

            await builder.RunAsync(async (s, c) =>
            {
                await runHandler(s).ConfigureAwait(false);
            }, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Builds and runs a host instance of the given type.
        /// </summary>
        /// <param name="builder">The builder to use</param>
        /// <param name="runHandler">The run handler</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Run(this IHostBuilder builder, Action<IServiceProvider> runHandler)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (runHandler == null) throw new ArgumentNullException(nameof(runHandler));

            builder.RunAsync((s, c) =>
            {
                runHandler(s);
                return HandlerHost.CompletedTask;
            }).ConfigureAwait(false).GetAwaiter().GetResult();
        }


        // ReSharper disable once ClassNeverInstantiated.Local
        private class HandlerHost : IHost
        {
            public static readonly Task CompletedTask = Task.FromResult(true);

            public Func<CancellationToken, Task> Handler { private get; set; }

            public async Task RunAsync(CancellationToken ct)
            {
                if (Handler != null)
                    await Handler(ct).ConfigureAwait(false);
            }
        }

        #endregion
    }
}
