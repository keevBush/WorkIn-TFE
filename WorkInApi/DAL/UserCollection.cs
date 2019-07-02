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
    public class UserCollection : IDataAccess<Demandeur>
    {
        public void DeleteItem(string id)
        {
            CosmoDbConfig.Instance
                    .Client
                    .DeleteDocumentAsync(UriFactory.
                    CreateDocumentUri(CosmoDbConfig.Instance.DatabaseId, "demandeurs", id));
        }

        public IEnumerable<Demandeur> GetAllItem()
        {
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(CosmoDbConfig.Instance.DatabaseId, "demandeurs");
            FeedOptions feedOptions = new FeedOptions { MaxItemCount = -1 };
            IDocumentQuery<Demandeur> demandeurs = CosmoDbConfig.Instance.Client.
                    CreateDocumentQuery<Demandeur>(collectionUri, feedOptions)
                    .AsDocumentQuery();
            List<Demandeur> listofDemandeur = new List<Demandeur>();
            while (demandeurs.HasMoreResults)
                listofDemandeur.AddRange(demandeurs.ExecuteNextAsync<Demandeur>().Result);
            return listofDemandeur;

        }

        public IEnumerable<Demandeur> GetItems(Expression<Func<Demandeur, bool>> where)
        {
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(CosmoDbConfig.Instance.DatabaseId, "demandeurs");
            FeedOptions feedOptions = new FeedOptions { MaxItemCount = -1 ,EnableCrossPartitionQuery=true};
            IDocumentQuery<Demandeur> demandeurs;
            if (where == null)
                demandeurs  = CosmoDbConfig.Instance.Client.
                    CreateDocumentQuery<Demandeur>(collectionUri, feedOptions)
                    .AsDocumentQuery();
            else
                demandeurs = CosmoDbConfig.Instance.Client.
                    CreateDocumentQuery<Demandeur>(collectionUri, feedOptions)
                    .Where(where)
                    .AsDocumentQuery();
            List<Demandeur> listofDemandeur = new List<Demandeur>();
            while (demandeurs.HasMoreResults)
                listofDemandeur.AddRange(demandeurs.ExecuteNextAsync<Demandeur>().Result);
            return listofDemandeur;
        }
        public void NewItems(params Demandeur[] items)
        {
            foreach (var i in items)
                CosmoDbConfig.Instance
                    .Client
                    .CreateDocumentAsync(UriFactory
                    .CreateDocumentCollectionUri(CosmoDbConfig.Instance.DatabaseId, "demandeurs"),
                    i);
        }

        public void UpdateItem(string id, Demandeur item)
        {
            if (string.IsNullOrEmpty(id))
                throw new ApplicationException("No demandeur id specified");
            Uri documentUri = UriFactory.CreateDocumentUri(CosmoDbConfig.Instance.DatabaseId, "demandeurs", id);
            CosmoDbConfig.Instance
                    .Client
                    .ReplaceDocumentAsync(documentUri, item);
        }
    }
}
