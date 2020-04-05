using Nib.Career.Core.Entities;
using Nib.Career.GrpcServer.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nib.Career.GrpcServer.Helpers
{
    public static class LocationExtension
    {
        public static LocationDto ToGetLocation(this Location location)
        {
            return new LocationDto { Id = location.Id, Name = location.City, State = location.State };
        }
    }
}
