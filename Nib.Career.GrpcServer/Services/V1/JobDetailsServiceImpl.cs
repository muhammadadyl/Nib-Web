using Grpc.Core;
using MediatR;
using System.Threading.Tasks;

namespace Nib.Career.GrpcServer.V1
{
    public partial class SearchJobRequest : IRequest<SearchJobResponse> { }
    public partial class GetJobDetailsRequest : IRequest<GetJobDetailsResponse> { }

    public class JobDetailsServiceImpl : JobDetailsService.JobDetailsServiceBase
    {
        private readonly IMediator _mediator;
        public JobDetailsServiceImpl(IMediator mediator) => _mediator = mediator;

        public override Task<SearchJobResponse> SearchJobs(SearchJobRequest request, ServerCallContext context) => _mediator.Send(request, context.CancellationToken);
        public override Task<GetJobDetailsResponse> GetJobDetails(GetJobDetailsRequest request, ServerCallContext context) => _mediator.Send(request, context.CancellationToken);
    }
}
