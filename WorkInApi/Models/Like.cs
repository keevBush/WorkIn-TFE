using Newtonsoft.Json;

namespace WorkInApi.Models
{
    public class Like
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("etat")]
        public bool Etat { get; set; }
    }
}