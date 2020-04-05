using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Nib.Career.Core.Entities;
using Nib.Career.Core.Errors;
using Nib.Career.Core.Services;
using Nib.Career.GrpcServer.Helpers;
using Nib.Career.GrpcServer.V1;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nib.Career.GrpcServer.Services.V1.Queries.JobDetails
{
    public class GetJobDetailsHandler : IRequestHandler<GetJobDetailsRequest, GetJobDetailsResponse>
    {
        private readonly IFileStorageService<Job> _fileStorageService;

        public GetJobDetailsHandler(IFileStorageService<Job> fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<GetJobDetailsResponse> Handle(GetJobDetailsRequest request, CancellationToken cancellationToken)
        {
            var listOfJobs = await _fileStorageService.GetAsync();

            var job = listOfJobs.FirstOrDefault(i => i.Id.ToString() == request.JobId);
            if (job == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ErrorCodes.Job.NotExists));
            }

            return job.ToJobDetails(true);
        }
    }
}
