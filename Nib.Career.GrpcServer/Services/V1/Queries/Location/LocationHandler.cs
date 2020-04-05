using MediatR;
using Nib.Career.Core.Services;
using Nib.Career.GrpcServer.Helpers;
using Nib.Career.GrpcServer.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nib.Career.GrpcServer.V1.Queries.Location
{
    public class LocationHandler : IRequestHandler<LocationRequest, GetLocationResponse>
    {
        private readonly ILocationApiService _locationService;

        public LocationHandler(ILocationApiService locationService)
        {
            _locationService = locationService;
        }

        public async Task<GetLocationResponse> Handle(LocationRequest request, CancellationToken cancellationToken)
        {
            var locations = await _locationService.GetAsync();

            var response = new GetLocationResponse();
            response.Location.Add(locations.Select(i => i.ToGetLocation()));

            return response;
        }
    }
}
