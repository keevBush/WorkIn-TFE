using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Models
{
    public class Entreprise
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("identite")]
        public EmployeurIdentite EmployeurIdentite { get; set; }
        [JsonProperty("propositions")]
        public IEnumerable<Proposition> Propositions { get; set; }
    }
}
