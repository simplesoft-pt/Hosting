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
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleSoft.Hosting
{
    internal sealed class HostRunContext<THost> : IHostRunContext<THost>
        where THost : class, IHost
    {
        private bool _disposed;
        private IServiceScope _serviceScope;
        private THost _host;
        
        public HostRunContext(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope ?? throw new ArgumentNullException(nameof(serviceScope));
        }

        /// <inheritdoc />
        ~HostRunContext()
        {
            Dispose(false);
        }

        public THost Host
        {
            get
            {
                FailIfDisposed();
                return _host ?? (_host = ServiceProvider.GetRequiredService<THost>());
            }
        }
        
        public IServiceProvider ServiceProvider
        {
            get
            {
                FailIfDisposed();
                return _serviceScope.ServiceProvider;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private void Dispose(bool disposing)
        {
            if(_disposed)
                return;

            if (disposing)
                _serviceScope?.Dispose();

            _serviceScope = null;
            _disposed = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FailIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(HostRunContext<THost>));
        }
    }
}