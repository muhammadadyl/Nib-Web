using Grpc.Core;
using MediatR;
using System.Threading.Tasks;

namespace Nib.Career.GrpcServer.V1
{
    public partial class LocationRequest : IRequest<GetLocationResponse> { }

    public class LocationServiceImpl : LocationService.LocationServiceBase
    {
        private readonly IMediator _mediator;
        
        public LocationServiceImpl(IMediator mediator) => _mediator = mediator;
        
        public override async Task<GetLocationResponse> GetLocation(LocationRequest request, ServerCallContext context) => await _mediator.Send(request, context.CancellationToken);
    }
}
