using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.IO;
using System;

namespace GPRCStreaming
{
    public class LocationService : LocationData.LocationDataBase
    {
        private readonly FileReader _fileReader;
        private readonly ILogger<LocationService> _logger;

        public LocationService(FileReader fileReader, ILogger<LocationService> logger)
        {
            _fileReader = fileReader;
            _logger = logger;
        }

        public override async Task GetLocations(GetLocationsRequest request, IServerStreamWriter<GetLocationsResponse> responseStream, ServerCallContext context)
        {
            try
            {
                _logger.LogInformation("Incoming request for GetLocationData");

                var locationData = await GetLocationData();
                var locationDataCount = locationData.Locations.Count;

                var dataLimit = request.DataLimit > locationDataCount ? locationDataCount : request.DataLimit;
                var random = new Random();

                for (var i = 0; i <= dataLimit - 1; i++)
                {
                    var item = locationData.Locations[i];

                    await responseStream.WriteAsync(new GetLocationsResponse
                    {
                        LatitudeE7 = item.LatitudeE7,
                        LongitudeE7 = item.LongitudeE7
                    });
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error occurred");
                throw;
            }
        }

        public override async Task GetAllLocations(GetAllLocationsRequest request, IServerStreamWriter<GetAllLocationsResponse> responseStream, ServerCallContext context)
        {
            _logger.LogInformation("Incoming request for GetAllLocationData");

            var locationData = await GetLocationData();
            var locations = locationData.Locations;

            foreach (var item in locations)
            {
                await responseStream.WriteAsync(new GetAllLocationsResponse
                {
                    LatitudeE7 = item.LatitudeE7,
                    LongitudeE7 = item.LongitudeE7
                });
            }
        }

        private async Task<RootLocation> GetLocationData()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = $"{currentDirectory}/Data/Location_History.json";

            var locationData = await _fileReader.ReadAllLinesAsync(filePath);

            return locationData;
        }
    }
}