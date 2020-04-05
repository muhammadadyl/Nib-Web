using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Nib.Career.Core.Entities
{
    public class Job : BaseEntity
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("location")]
        public Location Location { get; set; }
        [JsonPropertyName("createdDate")]
        public DateTimeOffset CreatedDate { get; set; }
    }
}
