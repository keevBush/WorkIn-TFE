﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Models
{
    public class Publication
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("details")]
        public PublicationDetails PublicationDetails { get; set; }
        [JsonProperty("commentaires")]
        public IEnumerable<CommentaireSend> Commentaires { get; set; }
        [JsonProperty("likes")]
        public IEnumerable<Like> Likes { get; set; }
        [JsonProperty("demandeur")]
        public DemandeurIdentite Demandeur { get; set; }
    }
    public enum TypePublication
    {
        Image,Video,Text
    }
}
