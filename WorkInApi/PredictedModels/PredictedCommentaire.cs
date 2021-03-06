﻿using Microsoft.ML.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkInApi.Models;

namespace WorkInApi.PredictedModels
{
    public class PredictedCommentaire:Commentaire
    {
        [JsonProperty("score")]//le score que le commentaire aura
        public float Score { get; set; } 
        [JsonProperty("sentiment_type")]
        [ColumnName("PredictedLabel")]//true= positive sentiment false=negative sentiment
        public bool SentimentType { get; set; }
        [JsonProperty("probabilite")]
        public float Probabilite { get; set; }
        public PredictedCommentaire() : base()
        {

        }
        public PredictedCommentaire(Commentaire commentaire): base()
        {
            this.Value = commentaire.Value;
            this.Id = commentaire.Id;
            this.Date = commentaire.Date;
            this.Sentiment = commentaire.Sentiment;
        }

    }
}
