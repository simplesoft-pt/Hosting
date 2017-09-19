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
using Microsoft.Extensions.Logging;
using SimpleSoft.Hosting.Params;

namespace SimpleSoft.Hosting
{
    /// <summary>
    /// Extensions for <see cref="IHostBuilder"/> instances.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds an handler to the <see cref="IHostBuilder.ConfigurationBuilderHandlers"/> collection.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="handler">The handler to add</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder ConfigureConfigurationBuilder<TBuilder>(this TBuilder builder, Action<ConfigurationBuilderParam> handler)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddConfigurationBuilderHandler(handler);
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
        public static TBuilder ConfigureConfiguration<TBuilder>(this TBuilder builder, Action<ConfigurationHandlerParam> handler)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddConfigurationHandler(handler);
            return builder;
        }

        /// <summary>
        /// Uses the given <see cref="ILoggerFactory"/> instance.
        /// </summary>
        /// <typeparam name="TBuilder">The builder type</typeparam>
        /// <param name="builder">The builder to use</param>
        /// <param name="loggerFactory">The logger factory to use</param>
        /// <returns>The builder instance</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TBuilder UseLoggerFactory<TBuilder>(this TBuilder builder, ILoggerFactory loggerFactory)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.LoggerFactory = loggerFactory;
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
        public static TBuilder ConfigureLoggerFactory<TBuilder>(this TBuilder builder, Action<LoggerFactoryHandlerParam> handler)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddLoggerFactoryHandler(handler);
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
        public static TBuilder ConfigureServiceCollection<TBuilder>(this TBuilder builder, Action<ServiceCollectionHandlerParam> handler)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddServiceCollectionHandler(handler);
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
        public static TBuilder UseServiceProviderBuilder<TBuilder>(this TBuilder builder, Func<ServiceProviderBuilderParam, IServiceProvider> providerBuilder)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ServiceProviderBuilder = providerBuilder;
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
        public static TBuilder Configure<TBuilder>(this TBuilder builder, Action<ConfigureHandlerParam> handler)
            where TBuilder : IHostBuilder
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.AddConfigureHandler(handler);
            return builder;
        }

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
                await ctx.Host.RunAsync(ct).ConfigureAwait(false);
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

            builder.RunHostAsync<THost>().ConfigureAwait(false)
                .GetAwaiter().GetResult();
        }
    }
}
