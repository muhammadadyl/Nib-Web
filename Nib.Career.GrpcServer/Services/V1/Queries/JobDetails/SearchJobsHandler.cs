using MediatR;
using Nib.Career.Core.Entities;
using Nib.Career.Core.Services;
using Nib.Career.GrpcServer.Helpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nib.Career.GrpcServer.V1.Queries.JobDetails
{
    public class SearchJobsHandler : IRequestHandler<SearchJobRequest, SearchJobResponse>
    {
        private readonly IFileStorageService<Job> _fileStorageService;

        public SearchJobsHandler(IFileStorageService<Job> fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public async Task<SearchJobResponse> Handle(SearchJobRequest request, CancellationToken cancellationToken)
        {
            if (request.Take == 0)
            {
                request.Take = 20;
            }

            var listOfJobs = await _fileStorageService.GetAsync();

            var jobs = (request.LocationId > 0) ? listOfJobs.Where(j => j.Location.Id == request.LocationId) : listOfJobs;

            var response = new SearchJobResponse { Count = jobs.Count() };

            response.Jobs.Add(jobs.Skip(request.Skip).Take(request.Take).Select(i => i.ToJobDetails()));

            return response;
        }
    }
}
