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
using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace SimpleSoft.Hosting
{
    /// <inheritdoc />
    public class HostingEnvironment : IHostingEnvironment
    {
        private readonly PhysicalFileProvider _contentRootFileProvider;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="name">The environment name, like 'Production'</param>
        /// <param name="applicationName">The application name</param>
        /// <param name="contentRootFileProvider">The content root file provider</param>
        public HostingEnvironment(string name, string applicationName, PhysicalFileProvider contentRootFileProvider)
        {
            Name = name;
            ApplicationName = applicationName;
            _contentRootFileProvider = contentRootFileProvider;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="name">The environment name, like 'Production'</param>
        /// <param name="applicationName">The application name</param>
        /// <param name="contentRootPath">The root path to be used by the <see cref="PhysicalFileProvider"/></param>
        public HostingEnvironment(string name, string applicationName, string contentRootPath)
            : this(name, applicationName, new PhysicalFileProvider(contentRootPath))
        {

        }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string ApplicationName { get; }

        /// <inheritdoc />
        public IFileProvider ContentRootFileProvider => _contentRootFileProvider;

        /// <inheritdoc />
        public string ContentRootPath => _contentRootFileProvider.Root;

        /// <summary>
        /// Builds a hosting environment with default values.
        /// 
        /// The environment name will be searched from the environment variables using the provided 
        /// key and if not found, the value 'Production' will be assigned.
        /// 
        /// The application name will be extracted from the entry assembly.
        /// 
        /// The current directory will be used for the file provider.
        /// </summary>
        /// <param name="environmentNameKey">The environment key to be searched</param>
        /// <returns>Hosting environment instance</returns>
        /// <see cref="ArgumentNullException"/>
        /// <see cref="ArgumentException"/>
        public static HostingEnvironment BuildDefault(string environmentNameKey = "environment")
        {
            if (environmentNameKey == null)
                throw new ArgumentNullException(nameof(environmentNameKey));
            if (string.IsNullOrWhiteSpace(environmentNameKey))
                throw new ArgumentException("Value cannot be whitespace.", nameof(environmentNameKey));

            var environment = Environment.GetEnvironmentVariable(environmentNameKey);
            if (string.IsNullOrWhiteSpace(environment))
                environment = "Production";

#if NETSTANDARD1_3

            return new HostingEnvironment(
                environment, null, Directory.GetCurrentDirectory());

#else
            return new HostingEnvironment(
                environment, Assembly.GetEntryAssembly().GetName().Name, Directory.GetCurrentDirectory());

#endif

        }
    }
}
