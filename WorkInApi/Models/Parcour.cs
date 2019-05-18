using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Models
{
    public class Parcour
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("institution")]
        public string NomInstitution { get; set; }
        [JsonProperty("annee")]
        public int Annee { get; set; }
        [JsonProperty("pourcentage")]
        public double Pourcentage { get; set; }
        [JsonProperty("logo")]
        public string Photo { get; set; }
    }
    public enum TypeInstitution
    {
        EcoleSuperieur,EcoleSecondaire,Universite
    }
}
