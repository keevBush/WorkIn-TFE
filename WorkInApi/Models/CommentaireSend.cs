using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Models
{
    public class CommentaireSend
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("commentaire")]
        public Commentaire Commentaire { get; set; }
        [JsonProperty("employeur")]
        public EmployeurIdentite EmployeurIdentite { get; set; }
    }
}
