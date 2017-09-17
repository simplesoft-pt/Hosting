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
        private readonly List<Action<ConfigurationBuilderHandlerParam>> _configurationBuilderHandlers = new List<Action<ConfigurationBuilderHandlerParam>>();
        private readonly List<Action<ConfigurationHandlerParam>> _configurationHandlers = new List<Action<ConfigurationHandlerParam>>();
        private readonly List<Action<LoggerFactoryHandlerParam>> _loggerFactoryHandlers = new List<Action<LoggerFactoryHandlerParam>>();

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

            _loggerFactory = null;
            _disposed = true;
        }

        #endregion

        /// <inheritdoc />
        public IHostingEnvironment Environment { get; }

        #region IConfigurationBuilder
        
        /// <inheritdoc />
        public IReadOnlyCollection<Action<ConfigurationBuilderHandlerParam>> ConfigurationBuilderHandlers => _configurationBuilderHandlers;

        /// <inheritdoc />
        public void AddConfigurationBuilderHandler(Action<ConfigurationBuilderHandlerParam> handler)
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

        /// <inheritdoc />
        public THost Build<THost>() where THost : IHost
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(HostBuilder));

            var configurationBuilder = BuildConfigurationBuilderUsingHandlers();

            var configurationRoot = BuildConfigurationRootUsingHandlers(configurationBuilder.Build());

            var loggerFactory = BuildLoggerFactoryUsingHandlers(configurationRoot);

            return default(THost);
        }

        #region Private

        private IConfigurationBuilder BuildConfigurationBuilderUsingHandlers()
        {
            var param = new ConfigurationBuilderHandlerParam(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"environmentName", Environment.Name},
                    {"contentRootPath", Environment.ContentRootPath}
                }), Environment);
            foreach (var handler in _configurationBuilderHandlers)
                handler(param);

            return param.Builder;
        }

        private IConfigurationRoot BuildConfigurationRootUsingHandlers(IConfigurationRoot configurationBuilder)
        {
            var param = new ConfigurationHandlerParam(configurationBuilder, Environment);
            foreach (var handler in _configurationHandlers)
                handler(param);

            return param.Configuration;
        }

        private ILoggerFactory BuildLoggerFactoryUsingHandlers(IConfiguration configuration)
        {
            var param = new LoggerFactoryHandlerParam(LoggerFactory, configuration, Environment);
            foreach (var handler in _loggerFactoryHandlers)
                handler(param);

            return param.LoggerFactory;
        }

        #endregion
    }
}
