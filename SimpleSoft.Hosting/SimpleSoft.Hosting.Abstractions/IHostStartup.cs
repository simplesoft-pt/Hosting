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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleSoft.Hosting.Params;

namespace SimpleSoft.Hosting
{
    /// <summary>
    /// Represents a startup class that can be by the <see cref="IHostBuilder"/>
    /// to configure <see cref="IHost"/> instances
    /// </summary>
    public interface IHostStartup
    {
        /// <summary>
        /// Configure the <see cref="IConfigurationBuilder"/> instance to 
        /// create the host configurations.
        /// </summary>
        /// <param name="param">Configuration arguments</param>
        void ConfigureConfigurationBuilder(IConfigurationBuilderParam param);

        /// <summary>
        /// Configure the <see cref="IConfigurationRoot"/> instance.
        /// </summary>
        /// <param name="param">Configuration arguments</param>
        void ConfigureConfiguration(IConfigurationHandlerParam param);

        /// <summary>
        /// Configure the <see cref="ILoggerFactory"/> instance.
        /// </summary>
        /// <param name="param">Configuration arguments</param>
        void ConfigureLoggerFactory(ILoggerFactoryHandlerParam param);

        /// <summary>
        /// Configure the <see cref="IServiceCollection"/> instance.
        /// </summary>
        /// <param name="param">Configuration arguments</param>
        void ConfigureServiceCollection(IServiceCollectionHandlerParam param);

        /// <summary>
        /// Builds the <see cref="IServiceProvider"/> instance.
        /// </summary>
        /// <param name="param">Builder params</param>
        /// <returns>The service provider</returns>
        IServiceProvider BuildServiceProvider(IServiceProviderBuilderParam param);

        /// <summary>
        /// Configures the application using the <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="param">Configuration arguments</param>
        void Configure(IConfigureHandlerParam param);
    }
}
