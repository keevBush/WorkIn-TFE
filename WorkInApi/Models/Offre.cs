using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Models
{
    public class Offre
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("typeOffre")]
        public TypeOffre TypeOffre { get; set; }
        [JsonProperty("nomPoste")]
        public string NomPoste { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("details")]
        public string Details { get; set; }
        [JsonProperty("deadline")]
        public DateTime? DeadLine { get; set; } = null;
        [JsonProperty("maxParticipant")]
        public int? MaxParticipant { get; set; }
        [JsonProperty("competences")]
        public IEnumerable<string> Competences { get; set; }
    }
    public enum TypeOffre
    {
        Public,Privee
    }
}
