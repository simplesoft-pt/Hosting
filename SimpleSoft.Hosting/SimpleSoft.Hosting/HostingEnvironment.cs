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
        /// <exception cref="ArgumentNullException"></exception>
        public HostingEnvironment(string name, string applicationName, PhysicalFileProvider contentRootFileProvider)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ApplicationName = applicationName ?? throw new ArgumentNullException(nameof(applicationName));
            _contentRootFileProvider = contentRootFileProvider ?? throw new ArgumentNullException(nameof(contentRootFileProvider));
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="name">The environment name, like 'Production'</param>
        /// <param name="applicationName">The application name</param>
        /// <param name="contentRootPath">The root path to be used by the <see cref="PhysicalFileProvider"/></param>
        /// <exception cref="ArgumentNullException"></exception>
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
    }
}
