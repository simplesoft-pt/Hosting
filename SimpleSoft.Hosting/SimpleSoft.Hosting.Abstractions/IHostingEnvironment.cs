using Microsoft.Extensions.FileProviders;

namespace SimpleSoft.Hosting
{
    /// <summary>
    /// Provides information about the hosting environment in which the 
    /// application is running.
    /// </summary>
    public interface IHostingEnvironment
    {
        /// <summary>
        /// The application name.
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// The <see cref="IFileProvider"/> pointing at <see cref="ContentRootPath"/>.
        /// </summary>
        IFileProvider ContentRootFileProvider { get; set; }

        /// <summary>
        /// The absolute path to the directory that contains the
        /// application files.
        /// </summary>
        string ContentRootPath { get; set; }

        /// <summary>
        /// The environment name.
        /// </summary>
        string Name { get; set; }
    }
}
