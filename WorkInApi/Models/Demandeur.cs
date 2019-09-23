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
        public virtual IEnumerable<Discussion> Messages { get; set; }
        [JsonProperty("publications")]
        public virtual IEnumerable<Publication> Publications { get; set; }
        [JsonProperty("competances")]
        public virtual IEnumerable<string> Competances { get; set; }
        [JsonProperty("parcours")]
        public virtual IEnumerable<Parcour> Parcours { get; set; }
        [JsonProperty("formations")]
        public virtual IEnumerable<Formation> Formations { get; set; }
        [JsonProperty("offres")]
        public virtual IEnumerable<Offre> Offres { get; set; }
        [JsonProperty("notifications")]
        public virtual IEnumerable<Notification> Notifications { get; set; }
        public Demandeur()
        {
            this.Publications = new List<Publication>();
            this.Messages = new List<Discussion>();
            this.Identite = new DemandeurIdentite();
            this.Competances = new List<string>();
            this.Parcours = new List<Parcour>();
            this.Formations = new List<Formation>();
            this.Offres = new List<Offre>();
            this.Notifications = new List<Notification>();
        }
    }
}
