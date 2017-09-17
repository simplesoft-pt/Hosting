using Microsoft.Extensions.FileProviders;

namespace SimpleSoft.Hosting
{
    /// <inheritdoc />
    public class HostingEnvironment : IHostingEnvironment
    {
        /// <inheritdoc />
        public string ApplicationName { get; set; }

        /// <inheritdoc />
        public IFileProvider ContentRootFileProvider { get; set; }

        /// <inheritdoc />
        public string ContentRootPath { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }
    }
}
