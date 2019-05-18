﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Models
{
    public class Notification
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("competance")]
        public string Msg { get; set; }
        [JsonProperty("page")]
        public string Page { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
