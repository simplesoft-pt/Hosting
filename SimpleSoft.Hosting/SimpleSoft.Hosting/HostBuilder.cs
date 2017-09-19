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
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SimpleSoft.Hosting.Params;

namespace SimpleSoft.Hosting
{
    /// <summary>
    /// Builder for <see cref="IHost"/> instances.
    /// </summary>
    public class HostBuilder : IHostBuilder, IDisposable
    {
        private bool _disposed;
        private ILoggerFactory _loggerFactory;
        private readonly List<Action<ConfigurationBuilderParam>> _configurationBuilderHandlers = new List<Action<ConfigurationBuilderParam>>();
        private readonly List<Action<ConfigurationHandlerParam>> _configurationHandlers = new List<Action<ConfigurationHandlerParam>>();
        private readonly List<Action<LoggerFactoryHandlerParam>> _loggerFactoryHandlers = new List<Action<LoggerFactoryHandlerParam>>();
        private readonly List<Action<ServiceCollectionHandlerParam>> _serviceCollectionHandlers = new List<Action<ServiceCollectionHandlerParam>>();
        private Func<ServiceProviderBuilderParam, IServiceProvider> _serviceProviderBuilder = p => p.ServiceCollection.BuildServiceProvider();
        private readonly List<Action<ConfigureHandlerParam>> _configureHandlers = new List<Action<ConfigureHandlerParam>>();

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="environment">The hosting environment information</param>
        /// <exception cref="ArgumentNullException"></exception>
        public HostBuilder(IHostingEnvironment environment)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            LoggerFactory = new LoggerFactory();
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="environmentNameKey">The environment key to be searched</param>
        /// <exception cref="ArgumentNullException"></exception>
        public HostBuilder(string environmentNameKey = "environment")
            : this(HostingEnvironment.BuildDefault(environmentNameKey))
        {

        }

        /// <inheritdoc />
        ~HostBuilder()
        {
            Dispose(false);
        }

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Invoked when disposing the instance.
        /// </summary>
        /// <param name="disposing">True if disposing, otherwise false</param>
        protected virtual void Dispose(bool disposing)
        {
            if(_disposed) return;

            if(disposing)
                _loggerFactory?.Dispose();

            _configurationBuilderHandlers.Clear();
            _configurationHandlers.Clear();
            _loggerFactoryHandlers.Clear();
            _serviceCollectionHandlers.Clear();
            _configureHandlers.Clear();

            _loggerFactory = null;
            _serviceProviderBuilder = null;

            _disposed = true;
        }

        #endregion

        /// <inheritdoc />
        public IHostingEnvironment Environment { get; }

        #region IConfigurationBuilder
        
        /// <inheritdoc />
        public IReadOnlyCollection<Action<ConfigurationBuilderParam>> ConfigurationBuilderHandlers => _configurationBuilderHandlers;

        /// <inheritdoc />
        public void AddConfigurationBuilderHandler(Action<ConfigurationBuilderParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _configurationBuilderHandlers.Add(handler);
        }

        #endregion

        #region IConfigurationRoot

        /// <inheritdoc />
        public IReadOnlyCollection<Action<ConfigurationHandlerParam>> ConfigurationHandlers => _configurationHandlers;

        /// <inheritdoc />
        public void AddConfigurationHandler(Action<ConfigurationHandlerParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _configurationHandlers.Add(handler);
        }

        #endregion

        #region ILoggerFactory

        /// <inheritdoc />
        public ILoggerFactory LoggerFactory
        {
            get => _loggerFactory;
            set => _loggerFactory = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <inheritdoc />
        public IReadOnlyCollection<Action<LoggerFactoryHandlerParam>> LoggerFactoryHandlers => _loggerFactoryHandlers;

        /// <inheritdoc />
        public void AddLoggerFactoryHandler(Action<LoggerFactoryHandlerParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _loggerFactoryHandlers.Add(handler);
        }

        #endregion

        #region IServiceCollection

        /// <inheritdoc />
        public IReadOnlyCollection<Action<ServiceCollectionHandlerParam>> ServiceCollectionHandlers => _serviceCollectionHandlers;

        /// <inheritdoc />
        public void AddServiceCollectionHandler(Action<ServiceCollectionHandlerParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _serviceCollectionHandlers.Add(handler);
        }

        #endregion

        /// <inheritdoc />
        public Func<ServiceProviderBuilderParam, IServiceProvider> ServiceProviderBuilder
        {
            get => _serviceProviderBuilder;
            set => _serviceProviderBuilder = value ?? throw new ArgumentNullException(nameof(value));
        }

        #region IServiceProvider

        /// <inheritdoc />
        public IReadOnlyCollection<Action<ConfigureHandlerParam>> ConfigureHandlers => _configureHandlers;

        /// <inheritdoc />
        /// <param name="handler">The handler to add</param>
        public void AddConfigureHandler(Action<ConfigureHandlerParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _configureHandlers.Add(handler);
        }

        #endregion

        /// <inheritdoc />
        public HostRunContext<THost> BuildRunContext<THost>() where THost : class, IHost
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(HostBuilder));

            var logger = LoggerFactory.CreateLogger<HostBuilder>();

            var configurationBuilder = BuildConfigurationBuilderUsingHandlers(logger);

            var configurationRoot = BuildConfigurationRootUsingHandlers(logger, configurationBuilder);

            var loggerFactory = BuildLoggerFactoryUsingHandlers(logger, configurationRoot);

            //  getting a new logger since configurations may have changed
            logger = loggerFactory.CreateLogger<HostBuilder>();

            var serviceCollection = BuildServiceCollectionUsingHandlers(logger, loggerFactory, configurationRoot);
            serviceCollection.TryAddScoped<THost>();

            var serviceProvider = BuildAndConfigureServiceProvider(logger, serviceCollection, loggerFactory, configurationRoot);

            return new HostRunContext<THost>(serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope());
        }

        #region Private

        private IConfigurationBuilder BuildConfigurationBuilderUsingHandlers(ILogger logger)
        {
            logger.LogDebug("Preparing configuration builder");

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"environmentName", Environment.Name},
                    {"contentRootPath", Environment.ContentRootPath}
                });

            logger.LogDebug("Running configuration builder handlers [Count: {configurationBuilderHandlersCount}]",
                _configurationBuilderHandlers.Count);

            var param = new ConfigurationBuilderParam(builder, Environment);
            foreach (var handler in _configurationBuilderHandlers)
                handler(param);

            return param.Builder;
        }

        private IConfigurationRoot BuildConfigurationRootUsingHandlers(ILogger logger, IConfigurationBuilder configurationBuilder)
        {
            logger.LogDebug("Building configurations root");

            var configurations = configurationBuilder.Build();

            logger.LogDebug("Running configurations handlers [Count: {configurationHandlersCount}]",
                _configurationHandlers.Count);

            var param = new ConfigurationHandlerParam(configurations, Environment);
            foreach (var handler in _configurationHandlers)
                handler(param);

            return param.Configuration;
        }

        private ILoggerFactory BuildLoggerFactoryUsingHandlers(ILogger logger, IConfiguration configuration)
        {
            logger.LogDebug("Running logger factory handlers [Count: {configurationHandlersCount}]",
                _configurationHandlers.Count);

            var param = new LoggerFactoryHandlerParam(LoggerFactory, configuration, Environment);
            foreach (var handler in _loggerFactoryHandlers)
                handler(param);

            return param.LoggerFactory;
        }

        private IServiceCollection BuildServiceCollectionUsingHandlers(ILogger logger, ILoggerFactory loggerFactory, IConfigurationRoot configuration)
        {
            logger.LogDebug("Configuring core services");

            var serviceCollection = new ServiceCollection()
                .AddSingleton(loggerFactory)
                .AddLogging()
                .AddSingleton(configuration)
                .AddSingleton<IConfiguration>(configuration)
                .AddSingleton(Environment);

            logger.LogDebug("Running service collection handlers [Count: {serviceCollectionHandlersCount}]",
                _serviceCollectionHandlers.Count);

            var param = new ServiceCollectionHandlerParam(serviceCollection, loggerFactory, configuration, Environment);
            foreach (var handler in _serviceCollectionHandlers)
                handler(param);

            return param.ServiceCollection;
        }

        private IServiceProvider BuildAndConfigureServiceProvider(ILogger logger, IServiceCollection serviceCollection, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            logger.LogDebug("Building service provider");

            var builderParam = new ServiceProviderBuilderParam(serviceCollection, loggerFactory, configuration, Environment);
            var serviceProvider = ServiceProviderBuilder(builderParam);

            logger.LogDebug("Running service provider handlers [Count: {configureHandlersCount}]",
                _configureHandlers.Count);

            var param = new ConfigureHandlerParam(serviceProvider, loggerFactory, configuration, Environment);
            foreach (var handler in _configureHandlers)
                handler(param);

            return param.ServiceProvider;
        }

        #endregion
    }
}
