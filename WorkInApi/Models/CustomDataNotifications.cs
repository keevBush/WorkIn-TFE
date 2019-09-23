using Newtonsoft.Json;
using System;

namespace WorkInApi.Models
{
    public class CustomDataNotifications
    {
        [JsonProperty("pageType")]
        public PageType PageType { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("idData")]
        public string IdData { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
    public enum PageType
    {
        Offre = 0, Message = 1, Publication = 2
    }
}