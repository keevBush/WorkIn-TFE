using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WorkInApi.Models;
using WorkInApi.Services;

namespace WorkInApi.DAL
{
    public class PropositionCollection : IDataAccess<Proposition>
    {
        public void DeleteItem(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Proposition> GetAllItem()
        {
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(CosmoDbConfig.Instance.DatabaseId, "propositions");
            FeedOptions feedOptions = new FeedOptions { MaxItemCount = -1 };
            IDocumentQuery<Proposition> demandeurs = CosmoDbConfig.Instance.Client.
                    CreateDocumentQuery<Proposition>(collectionUri, feedOptions)
                    .AsDocumentQuery();
            List<Proposition> listofEntreprise = new List<Proposition>();
            while (demandeurs.HasMoreResults)
                listofEntreprise.AddRange(demandeurs.ExecuteNextAsync<Proposition>().Result);
            return listofEntreprise;
        }

        public IEnumerable<Proposition> GetItems(Expression<Func<Proposition, bool>> where)
        {
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(CosmoDbConfig.Instance.DatabaseId, "propositions");
            FeedOptions feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
            IDocumentQuery<Proposition> propositions;
            if (where == null)
                propositions = CosmoDbConfig.Instance.Client.
                    CreateDocumentQuery<Proposition>(collectionUri, feedOptions)
                    .AsDocumentQuery();
            else
                propositions = CosmoDbConfig.Instance.Client.
                    CreateDocumentQuery<Proposition>(collectionUri, feedOptions)
                    .Where(where)
                    .AsDocumentQuery();
            List<Proposition> listofPropositions = new List<Proposition>();
            while (propositions.HasMoreResults)
                listofPropositions.AddRange(propositions.ExecuteNextAsync<Proposition>().Result);
            return listofPropositions;
        }

        public void NewItems(params Proposition[] items)
        {
            foreach (var i in items)
                CosmoDbConfig.Instance
                    .Client
                    .CreateDocumentAsync(UriFactory
                    .CreateDocumentCollectionUri(CosmoDbConfig.Instance.DatabaseId, "propositions"),
                    i);
        }

        public void UpdateItem(string id, Proposition item)
        {
            throw new NotImplementedException();
        }
    }
}
