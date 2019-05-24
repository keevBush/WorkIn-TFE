using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Models
{
    public class Demandeur
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("identite")]
        public DemandeurIdentite Identite { get; set; }
        [JsonProperty("experienceprofessionnelles")]
        public IEnumerable<ExperienceProfessionnelle> ExperienceProfessionnelles { get; set; }
        [JsonProperty("messages")]
        public IEnumerable<Discussion> Messages { get; set; }
        [JsonProperty("competances")]
        public IEnumerable<Competance> Competances { get; set; }
        [JsonProperty("parcours")]
        public IEnumerable<Parcour> Parcours { get; set; }
        [JsonProperty("formations")]
        public IEnumerable<Formation> Formations { get; set; }
        [JsonProperty("offres")]
        public IEnumerable<Offre> Offres { get; set; }
    }
}
