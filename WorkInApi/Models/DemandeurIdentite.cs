using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Models
{
    public class DemandeurIdentite
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("nom")]
        public string Nom { get; set; }
        [JsonProperty("postnom")]
        public string Postnom { get; set; }
        [JsonProperty("prenom")]
        public string Prenom { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("motDePasse")]
        public string Password { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("adresse")]
        public string Adresse { get; set; }
        [JsonProperty("telephone")]
        public string Telephone { get; set; }
        [JsonProperty("nationalite")]
        public string Nationnalite { get; set; }
        [JsonProperty("genre")]
        public Genre Genre { get; set; }
        [JsonProperty("naissance")]
        public DateTime Naissance { get; set; }
        [JsonProperty("langues")]
        public IEnumerable<string> LanguesParle { get; set; }
        [JsonProperty("aPropos")]
        public string Apropos { get; set; }
        [JsonProperty("localisation")]
        public string Localisation { get; set; }
        [JsonProperty("is_verified")]
        public bool IsVerified { get; set; }
    }
    public enum Genre
    {
        Homme,Femme
    }
}