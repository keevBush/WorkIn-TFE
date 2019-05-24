using Newtonsoft.Json;
using System.Collections.Generic;

namespace WorkInApi.Models
{
    public class Proposition
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("offre")]
        public Offre Offre { get; set; }
        [JsonProperty("demandeur")]
        public IEnumerable<DemandeurIdentite> DemandeurIdentites { get; set; }
    }
}