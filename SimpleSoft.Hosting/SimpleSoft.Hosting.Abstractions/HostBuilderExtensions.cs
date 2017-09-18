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
    }
}
