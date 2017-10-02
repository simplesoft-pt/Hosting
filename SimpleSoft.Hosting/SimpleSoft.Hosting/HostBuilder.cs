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
using System.Runtime.CompilerServices;
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
    public class HostBuilder : IHostBuilder
    {
        private bool _disposed;
        private bool _disposeLoggerFactory = true;
        private ILogger<HostBuilder> _logger;
        private readonly List<Action<IConfigurationBuilderParam>> _configurationBuilderHandlers = new List<Action<IConfigurationBuilderParam>>();
        private readonly List<Action<IConfigurationHandlerParam>> _configurationHandlers = new List<Action<IConfigurationHandlerParam>>();
        private readonly List<Action<ILoggerFactoryHandlerParam>> _loggerFactoryHandlers = new List<Action<ILoggerFactoryHandlerParam>>();
        private readonly List<Action<IServiceCollectionHandlerParam>> _serviceCollectionHandlers = new List<Action<IServiceCollectionHandlerParam>>();
        private Func<IServiceProviderBuilderParam, IServiceProvider> _serviceProviderBuilder = p => p.ServiceCollection.BuildServiceProvider();
        private readonly List<Action<IConfigureHandlerParam>> _configureHandlers = new List<Action<IConfigureHandlerParam>>();

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="environment">The hosting environment information</param>
        /// <exception cref="ArgumentNullException"></exception>
        public HostBuilder(IHostingEnvironment environment)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));

            LoggerFactory = new LoggerFactory();
            _logger = LoggerFactory.CreateLogger<HostBuilder>();
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="environmentNameKey">The environment key to be searched</param>
        /// <exception cref="ArgumentNullException"></exception>
        public HostBuilder(string environmentNameKey = Constants.EnvironmentNameKeyDefault)
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

            if (disposing)
            {
                if (_disposeLoggerFactory)
                {
                    LoggerFactory?.Dispose();
                }
            }

            _configurationBuilderHandlers.Clear();
            _configurationHandlers.Clear();
            _loggerFactoryHandlers.Clear();
            _serviceCollectionHandlers.Clear();
            _configureHandlers.Clear();

            LoggerFactory = null;
            _logger = null;
            _serviceProviderBuilder = null;

            _disposed = true;
        }

        #endregion

        /// <inheritdoc />
        public IHostingEnvironment Environment { get; }

        #region IConfigurationBuilder
        
        /// <inheritdoc />
        public IReadOnlyCollection<Action<IConfigurationBuilderParam>> ConfigurationBuilderHandlers => _configurationBuilderHandlers;

        /// <inheritdoc />
        public void AddConfigurationBuilderHandler(Action<IConfigurationBuilderParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            FailIfDisposed();
            _configurationBuilderHandlers.Add(handler);
        }

        #endregion

        #region IConfigurationRoot

        /// <inheritdoc />
        public IReadOnlyCollection<Action<IConfigurationHandlerParam>> ConfigurationHandlers => _configurationHandlers;

        /// <inheritdoc />
        public void AddConfigurationHandler(Action<IConfigurationHandlerParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            FailIfDisposed();
            _configurationHandlers.Add(handler);
        }

        #endregion

        #region ILoggerFactory

        /// <inheritdoc />
        public ILoggerFactory LoggerFactory { get; private set; }

        /// <inheritdoc />
        public void SetLoggerFactory(ILoggerFactory loggerFactory, bool disposeFactory = true)
        {
            FailIfDisposed();

            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _disposeLoggerFactory = disposeFactory;
            _logger = LoggerFactory.CreateLogger<HostBuilder>();
        }

        /// <inheritdoc />
        public IReadOnlyCollection<Action<ILoggerFactoryHandlerParam>> LoggerFactoryHandlers => _loggerFactoryHandlers;

        /// <inheritdoc />
        public void AddLoggerFactoryHandler(Action<ILoggerFactoryHandlerParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _loggerFactoryHandlers.Add(handler);
        }

        #endregion

        #region IServiceCollection

        /// <inheritdoc />
        public IReadOnlyCollection<Action<IServiceCollectionHandlerParam>> ServiceCollectionHandlers => _serviceCollectionHandlers;

        /// <inheritdoc />
        public void AddServiceCollectionHandler(Action<IServiceCollectionHandlerParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            FailIfDisposed();
            _serviceCollectionHandlers.Add(handler);
        }

        #endregion

        /// <inheritdoc />
        public Func<IServiceProviderBuilderParam, IServiceProvider> ServiceProviderBuilder
        {
            get => _serviceProviderBuilder;
            set
            {
                FailIfDisposed();
                _serviceProviderBuilder = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        #region IServiceProvider

        /// <inheritdoc />
        public IReadOnlyCollection<Action<IConfigureHandlerParam>> ConfigureHandlers => _configureHandlers;

        /// <inheritdoc />
        /// <param name="handler">The handler to add</param>
        public void AddConfigureHandler(Action<IConfigureHandlerParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            FailIfDisposed();
            _configureHandlers.Add(handler);
        }

        #endregion

        /// <inheritdoc />
        public IHostRunContext<THost> BuildRunContext<THost>() where THost : class, IHost
        {
            FailIfDisposed();

            using (_logger.BeginScope("HostType:{hostType}", typeof(THost)))
            {
                _logger.LogDebug(
                    "Building run context [Environment:'{environment}' ContentRootPath:'{contentRootPath}' ApplicationName:'{applicationName}']",
                    Environment.Name, Environment.ContentRootPath, Environment.ApplicationName);

                var configurationBuilder = BuildConfigurationBuilderUsingHandlers();

                var configurationRoot = BuildConfigurationRootUsingHandlers(configurationBuilder);

                ConfigureLoggerFactoryUsingHandlers(configurationRoot);

                var serviceCollection = BuildServiceCollectionUsingHandlers(configurationRoot);
                serviceCollection.TryAddScoped<THost>();

                var serviceProvider = BuildAndConfigureServiceProvider(serviceCollection, configurationRoot);

                var ctx = new HostRunContext<THost>(
                    serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope());

                _logger.LogInformation("Built a new context to run the host [Id:{hostRunContextId}]", ctx.Id);

                return ctx;
            }
        }

        #region Private

        private IConfigurationBuilder BuildConfigurationBuilderUsingHandlers()
        {
            _logger.LogDebug("Preparing configuration builder");

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"environment:name", Environment.Name},
                    {"environment:contentRootPath", Environment.ContentRootPath}
                });

            _logger.LogDebug("Running configuration builder handlers [Count: {configurationBuilderHandlersCount}]",
                _configurationBuilderHandlers.Count);

            var param = new ConfigurationBuilderParam(builder, Environment);
            foreach (var handler in _configurationBuilderHandlers)
                handler(param);

            return param.Builder;
        }

        private IConfigurationRoot BuildConfigurationRootUsingHandlers(IConfigurationBuilder configurationBuilder)
        {
            _logger.LogDebug("Building configurations root");

            var configurations = configurationBuilder.Build();

            _logger.LogDebug("Running configurations handlers [Count: {configurationHandlersCount}]",
                _configurationHandlers.Count);

            var param = new ConfigurationHandlerParam(configurations, Environment);
            foreach (var handler in _configurationHandlers)
                handler(param);

            return param.Configuration;
        }

        private void ConfigureLoggerFactoryUsingHandlers(IConfiguration configuration)
        {
            _logger.LogDebug("Running logger factory handlers [Count: {configurationHandlersCount}]",
                _configurationHandlers.Count);

            if (_configurationHandlers.Count == 0)
                return;

            var param = new LoggerFactoryHandlerParam(LoggerFactory, configuration, Environment);
            foreach (var handler in _loggerFactoryHandlers)
                handler(param);
            _logger.LogDebug("Finished the logger factory handler configuration");
        }

        private IServiceCollection BuildServiceCollectionUsingHandlers(IConfigurationRoot configuration)
        {
            _logger.LogDebug("Configuring core services");

            var serviceCollection = new ServiceCollection()
                .AddSingleton(LoggerFactory)
                .AddLogging()
                .AddSingleton(configuration)
                .AddSingleton(Environment);

            _logger.LogDebug("Running service collection handlers [Count: {serviceCollectionHandlersCount}]",
                _serviceCollectionHandlers.Count);

            var param = new ServiceCollectionHandlerParam(serviceCollection, LoggerFactory, configuration, Environment);
            foreach (var handler in _serviceCollectionHandlers)
                handler(param);

            return param.ServiceCollection;
        }

        private IServiceProvider BuildAndConfigureServiceProvider(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _logger.LogDebug("Building service provider");

            var builderParam = new ServiceProviderBuilderParam(serviceCollection, LoggerFactory, configuration, Environment);
            var serviceProvider = ServiceProviderBuilder(builderParam);

            _logger.LogDebug("Running service provider handlers [Count: {configureHandlersCount}]",
                _configureHandlers.Count);

            var param = new ConfigureHandlerParam(serviceProvider, LoggerFactory, configuration, Environment);
            foreach (var handler in _configureHandlers)
                handler(param);

            return param.ServiceProvider;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FailIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(HostBuilder));
        }

        #endregion
    }
}
