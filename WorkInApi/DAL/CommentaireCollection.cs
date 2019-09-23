using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorkInApi.MachineLearning;
using WorkInApi.PredictedModels;
using WorkInApi.Services;

namespace WorkInApi.DAL
{
    public class CommentaireCollection : IDataAccess<PredictedCommentaire>
    {
        public void DeleteItem(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PredictedCommentaire> GetAllItem()
        {
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(CosmoDbConfig.Instance.DatabaseId, "commentaires");
            FeedOptions feedOptions = new FeedOptions { MaxItemCount = -1 };
            IDocumentQuery<PredictedCommentaire> demandeurs = CosmoDbConfig.Instance.Client.
                    CreateDocumentQuery<PredictedCommentaire>(collectionUri, feedOptions)
                    .AsDocumentQuery();
            List<PredictedCommentaire> listofEntreprise = new List<PredictedCommentaire>();
            while (demandeurs.HasMoreResults)
                listofEntreprise.AddRange(demandeurs.ExecuteNextAsync<PredictedCommentaire>().Result);
            return listofEntreprise;
        }

        public IEnumerable<PredictedCommentaire> GetItems(Expression<Func<PredictedCommentaire, bool>> where)
        {
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(CosmoDbConfig.Instance.DatabaseId, "commentaires");
            FeedOptions feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
            IDocumentQuery<PredictedCommentaire> demandeurs;
            if (where == null)
                demandeurs = CosmoDbConfig.Instance.Client.
                    CreateDocumentQuery<PredictedCommentaire>(collectionUri, feedOptions)
                    .AsDocumentQuery();
            else
                demandeurs = CosmoDbConfig.Instance.Client.
                    CreateDocumentQuery<PredictedCommentaire>(collectionUri, feedOptions)
                    .Where(where)
                    .AsDocumentQuery();
            List<PredictedCommentaire> listofEntreprise = new List<PredictedCommentaire>();
            while (demandeurs.HasMoreResults)
                listofEntreprise.AddRange(demandeurs.ExecuteNextAsync<PredictedCommentaire>().Result);
            return listofEntreprise;
        }

        public void NewItems(params PredictedCommentaire[] items)
        {
            foreach (var i in items)
                CosmoDbConfig.Instance
                    .Client
                    .CreateDocumentAsync(UriFactory
                    .CreateDocumentCollectionUri(CosmoDbConfig.Instance.DatabaseId, "commentaires"),
                    i);
        }

        public void UpdateItem(string id, PredictedCommentaire item)
        {
            throw new NotImplementedException();
        }
    }
}
