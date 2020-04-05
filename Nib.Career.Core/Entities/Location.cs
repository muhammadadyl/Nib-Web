using System.Text.Json.Serialization;

namespace Nib.Career.Core.Entities
{
    public class Location : BaseEntity
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string City { get; set; }
        [JsonPropertyName("state")]
        public string State { get; set; }
    }
}