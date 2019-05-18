using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Models
{
    public class Formation
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("nomFormation")]
        public string NomFormation { get; set; }
        [JsonProperty("nomInstitution")]
        public string NomInstitution { get; set; }
        [JsonProperty("typeFormation")]
        public string TypeFormation { get; set; }
        [JsonProperty("annee")]
        public int Annee { get; set; }
        [JsonProperty("photo")]
        public string PhotoCertificat { get; set; }
        [JsonProperty("numero")]
        public string NumeroCertificat { get; set; }
    }
}
