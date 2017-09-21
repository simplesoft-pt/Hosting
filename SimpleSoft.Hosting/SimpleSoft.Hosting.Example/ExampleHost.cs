using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SimpleSoft.Hosting.Example
{
    public class ExampleHost : IHost
    {
        private readonly IHostingEnvironment _env;
        private readonly ExampleHostOptions _options;
        private readonly ILogger<ExampleHost> _logger;

        public ExampleHost(IHostingEnvironment env, ExampleHostOptions options, ILogger<ExampleHost> logger)
        {
            _env = env;
            _options = options;
            _logger = logger;
        }

        public Task RunAsync(CancellationToken ct)
        {
            _logger.LogInformation(
                "EnvironmentName:'{environmentName}' ContentRootPath:'{contentRootPath}' CacheTimeoutInMs:{cacheTimeoutInMs}",
                _env.Name, _env.ContentRootPath, _options.CacheTimeoutInMs);

            return Task.CompletedTask;
        }
    }
}