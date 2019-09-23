using Newtonsoft.Json;

namespace WorkInApi.Models
{
    public class Publicite
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("type")]
        public MediaType Type { get; set; }
    }
    public enum MediaType
    {
        Video,Image
    }
}