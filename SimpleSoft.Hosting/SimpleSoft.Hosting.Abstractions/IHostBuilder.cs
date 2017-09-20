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
using Microsoft.Extensions.Logging;
using SimpleSoft.Hosting.Params;

namespace SimpleSoft.Hosting
{
    /// <summary>
    /// Builder for <see cref="IHost"/> instances.
    /// </summary>
    public interface IHostBuilder : IDisposable
    {
        /// <summary>
        /// The builder hosting environment.
        /// </summary>
        IHostingEnvironment Environment { get; }

        #region IConfigurationBuilder

        /// <summary>
        /// Collection of handlers that configure the <see cref="IConfigurationBuilder"/>
        /// that will be used to generate the <see cref="IConfigurationRoot"/>.
        /// </summary>
        IReadOnlyCollection<Action<IConfigurationBuilderParam>> ConfigurationBuilderHandlers { get; }

        /// <summary>
        /// Adds an handler to the <see cref="ConfigurationBuilderHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddConfigurationBuilderHandler(Action<IConfigurationBuilderParam> handler);

        #endregion

        #region IConfigurationRoot

        /// <summary>
        /// Collection of handlers that configure the <see cref="IConfigurationRoot"/>.
        /// </summary>
        IReadOnlyCollection<Action<IConfigurationHandlerParam>> ConfigurationHandlers { get; }

        /// <summary>
        /// Adds an handler to the <see cref="ConfigurationHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddConfigurationHandler(Action<IConfigurationHandlerParam> handler);

        #endregion

        #region ILoggerFactory

        /// <summary>
        /// The builder logger factory.
        /// </summary>
        ILoggerFactory LoggerFactory { get; set; }

        /// <summary>
        /// Collection of handlers that configure the <see cref="ILoggerFactory"/>.
        /// </summary>
        IReadOnlyCollection<Action<ILoggerFactoryHandlerParam>> LoggerFactoryHandlers { get; }

        /// <summary>
        /// Adds an handler to the <see cref="LoggerFactoryHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddLoggerFactoryHandler(Action<ILoggerFactoryHandlerParam> handler);

        #endregion

        #region IServiceCollection

        /// <summary>
        /// Collection of handlers that configure the <see cref="IServiceCollection"/>.
        /// </summary>
        IReadOnlyCollection<Action<IServiceCollectionHandlerParam>> ServiceCollectionHandlers { get; }

        /// <summary>
        /// Adds an handler to the <see cref="ServiceCollectionHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddServiceCollectionHandler(Action<IServiceCollectionHandlerParam> handler);

        #endregion

        /// <summary>
        /// Builds the <see cref="IServiceProvider"/>.
        /// </summary>
        Func<IServiceProviderBuilderParam, IServiceProvider> ServiceProviderBuilder { get; set; }

        #region IServiceProvider

        /// <summary>
        /// Collection of handlers that configure the <see cref="IServiceProvider"/>.
        /// </summary>
        IReadOnlyCollection<Action<IConfigureHandlerParam>> ConfigureHandlers { get; }

        /// <summary>
        /// Adds an handler to the <see cref="ConfigureHandlers"/> collection.
        /// </summary>
        /// <param name="handler">The handler to add</param>
        void AddConfigureHandler(Action<IConfigureHandlerParam> handler);

        #endregion

        /// <summary>
        /// Builds a run context for the given host type.
        /// </summary>
        /// <typeparam name="THost">The host type</typeparam>
        /// <returns>Run context for the host</returns>
        IHostRunContext<THost> BuildRunContext<THost>() where THost : class, IHost;
    }
}
