using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkInApi.DAL;
using WorkInApi.Models;

namespace WorkInApi.MachineLearning
{
    public class Prediction
    {
        public static (double,DemandeurIdentite) AttribScoreForOneCandidat(Demandeur demandeur)
        {
            var publicationId = demandeur.Publications.Select(p => p.Id);
            var PublicationsCollection = new PublicationCollection();
            var publications = PublicationsCollection.GetItems(p => publicationId.Contains(p.Id)).ToList();
            var like = 0;
            var ranking = 0.0;
            foreach(var p in publications)
            {
               foreach(var c in p.Commentaires)
                {
                    var predictedCommentaire = new PredictedModels.PredictedCommentaire(c.Commentaire);
                    predictedCommentaire = predictedCommentaire.AttribScoreAndTypeOfComment();
                    ranking += predictedCommentaire.Score;
                }
                like += p.Likes.Where(l=> l.Etat == true).Count();
            }
            return (ranking+like, demandeur.Identite);
        }
    }
}
