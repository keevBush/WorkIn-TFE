﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Models
{
    public class ExperienceProfetionnelle
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("posteOccupee")]
        public string PosteOccupee { get; set; }
        [JsonProperty("debut")]
        public DateTime Debut { get; set; }
        [JsonProperty("fin")]
        public DateTime? Fin { get; set; }
        [JsonProperty("societe")]
        public string NomSociete { get; set; }
        [JsonProperty("decription")]
        public string DescriptionPoste { get; set; }
    }
}
