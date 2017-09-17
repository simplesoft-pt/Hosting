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

namespace SimpleSoft.Hosting.Params
{
    /// <summary>
    /// The parameter for handlers that configure the <see cref="IConfigurationBuilder"/>
    /// that will be used to generate the <see cref="IConfigurationRoot"/>.
    /// </summary>
    public sealed class ConfigurationBuilderHandlerParam
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="builder">The configuration builder</param>
        /// <param name="environment">The hosting environment</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConfigurationBuilderHandlerParam(IConfigurationBuilder builder, IHostingEnvironment environment)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        /// <summary>
        /// The configuration builder.
        /// </summary>
        public IConfigurationBuilder Builder { get; }

        /// <summary>
        /// The hosting environment.
        /// </summary>
        public IHostingEnvironment Environment { get; }
    }
}