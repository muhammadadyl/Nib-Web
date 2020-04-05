using Google.Protobuf.WellKnownTypes;
using Nib.Career.Core.Entities;
using Nib.Career.GrpcServer.V1;

namespace Nib.Career.GrpcServer.Helpers
{
    public static class JobExtension
    {
        public static GetJobDetailsResponse ToJobDetails(this Job job, bool isFullDescription = false)
        {
            if (job != null)
            {
                var description = job.Description;
                if (!isFullDescription && description.Length > 130) 
                {
                    description = description.Substring(0, 130);
                }

                return new GetJobDetailsResponse
                {
                    JobId = job.Id.ToString(),
                    Title = job.Title,
                    Description = description,
                    CreatedDate = Timestamp.FromDateTimeOffset(job.CreatedDate),
                    Location = $"{job.Location.City}, {job.Location.State}"
                };
            }
            return null;
        }
    }
}
