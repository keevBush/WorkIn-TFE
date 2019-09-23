using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Models
{
    public class PublicationDetails
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("legende")]
        public string Legende { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("tag")]
        public IEnumerable<string> Tags { get; set; }
        [JsonProperty("typePublication")]   
        public TypePublication TypePublication { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
