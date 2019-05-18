using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Models
{
    public class Competance
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("competance")]
        public string NomCompetance { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
