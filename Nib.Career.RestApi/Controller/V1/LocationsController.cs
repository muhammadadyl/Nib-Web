using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nib.Career.GrpcServer.V1;

namespace Nib.Career.RestApi.Controller.V1
{
    [ApiController]
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/locations")]
    public class LocationsController : ControllerBase
    {
        private readonly ILogger<LocationsController> _logger;
        private readonly LocationService.LocationServiceClient _locationService;

        public LocationsController(ILogger<LocationsController> logger, LocationService.LocationServiceClient locationService)
        {
            _logger = logger;
            _locationService = locationService;
        }

        [Route("")]
        [HttpGet]
        [ProducesResponseType(typeof(LocationResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLocations()
        {
            _logger.LogInformation("Getting locations");

            var response = await _locationService.GetLocationAsync(new LocationRequest());

            var responseDto = new LocationResponseDto
            {
                Items = response.Location.Select(x => new LocationResponseDto.LocationItem
                {
                    Id = x.Id,
                    City = x.Name,
                    State = x.State
                })
            };

            return Ok(responseDto);
        }
    }

    public class LocationResponseDto
    {
        public IEnumerable<LocationItem> Items { get; set; }

        public class LocationItem
        {
            public int Id { get; set; }
            public string City { get; set; }
            public string State { get; set; }
        }
    }
}