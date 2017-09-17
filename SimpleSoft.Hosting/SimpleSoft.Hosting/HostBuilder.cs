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
using SimpleSoft.Hosting.Params;

namespace SimpleSoft.Hosting
{
    /// <summary>
    /// Builder for <see cref="IHost"/> instances.
    /// </summary>
    public class HostBuilder : IHostBuilder, IDisposable
    {
        private bool _disposed;
        private readonly List<Action<ConfigurationBuilderConfiguratorParam>> _configurationBuilderHandlers = new List<Action<ConfigurationBuilderConfiguratorParam>>();

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="environment">The hosting environment information</param>
        /// <exception cref="ArgumentNullException"></exception>
        public HostBuilder(IHostingEnvironment environment)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
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


            _configurationBuilderHandlers.Clear();

            _disposed = true;
        }

        #endregion

        /// <inheritdoc />
        public IHostingEnvironment Environment { get; }

        /// <inheritdoc />
        public IReadOnlyCollection<Action<ConfigurationBuilderConfiguratorParam>> ConfigurationBuilderHandlers => _configurationBuilderHandlers;

        /// <inheritdoc />
        public void AddConfigurationBuilderHandler(Action<ConfigurationBuilderConfiguratorParam> handler)
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            _configurationBuilderHandlers.Add(handler);
        }
    }
}
