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
    [Route("v{version:apiVersion}/jobs")]
    public class JobsController : ControllerBase
    {
        private readonly ILogger<JobsController> _logger;
        private readonly JobDetailsService.JobDetailsServiceClient _jobDetailsService;

        public JobsController(ILogger<JobsController> logger, JobDetailsService.JobDetailsServiceClient jobDetailsService)
        {
            _logger = logger;
            _jobDetailsService = jobDetailsService;
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType(typeof(SearchJobResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchJobs(SearchJobRequestDto request)
        {
            _logger.LogInformation("Searching Jobs {@request}", new { request.LocationId });

            var response = await _jobDetailsService.SearchJobsAsync(new SearchJobRequest { LocationId = request.LocationId, Skip = request.Skip, Take = request.Take });

            var responseDto = new SearchJobResponseDto
            {
                Items = response.Jobs.Select(x => new JobItem
                {
                    Id = x.JobId,
                    Title = x.Title,
                    Description = x.Description,
                    Location = x.Location,
                    CreatedDate = x.CreatedDate.ToDateTimeOffset()
                }),
                Count = response.Count
            };

            return Ok(responseDto);
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(JobItem), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetJobDetails(string id)
        {
            _logger.LogInformation("Getting Jobs {@request}", new { id });

            var response = await _jobDetailsService.GetJobDetailsAsync(new GetJobDetailsRequest { JobId = id });

            return Ok(new JobItem
            {
                Id = response.JobId,
                Title = response.Title,
                Description = response.Description,
                Location = response.Location,
                CreatedDate = response.CreatedDate.ToDateTimeOffset()
            });
        }
    }

    public class SearchJobRequestDto
    {
        public int LocationId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class SearchJobResponseDto
    {
        public IEnumerable<JobItem> Items { get; set; }
        public int Count { get; set; }
    }

    public class JobItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}