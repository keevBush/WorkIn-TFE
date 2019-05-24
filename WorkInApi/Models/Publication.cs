﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Models
{
    public class Publication
    {
        [JsonProperty("publicationDetails")]
        public PublicationDetails PublicationDetails { get; set; }
        [JsonProperty("commentaires")]
        public IEnumerable<Commentaire> Commentaires { get; set; }
        [JsonProperty("likes")]
        public IEnumerable<Like> Likes { get; set; }
    }
    public enum TypePublication
    {
        Image,Video,Text
    }
}
