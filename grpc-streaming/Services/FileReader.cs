using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GPRCStreaming
{
    public class FileReader
    {
        private readonly ILogger<FileReader> _logger;

        private RootLocation _locationData;

        public FileReader(ILogger<FileReader> logger)
        {
            _logger = logger;
        }

        public async Task<RootLocation> ReadAllLinesAsync(string filePath)
        {
            if (_locationData == null)
            {
                _logger.LogInformation($"Reading contents of {filePath} file");

                var locationDataText = await File.ReadAllTextAsync(filePath);
                _locationData = JsonConvert.DeserializeObject<RootLocation>(locationDataText);

                _logger.LogInformation($"{_locationData.Locations.Count} records found");
            }

            return _locationData;
        }
    }
}