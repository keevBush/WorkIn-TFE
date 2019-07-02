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
    public class EntrepriseCollection : IDataAccess<Entreprise>
    {
        public void DeleteItem(string id)
        {
            CosmoDbConfig.Instance
                    .Client
                    .DeleteDocumentAsync(UriFactory.
                    CreateDocumentUri(CosmoDbConfig.Instance.DatabaseId, "entreprises", id));
        }

        public IEnumerable<Entreprise> GetAllItem()
        {
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(CosmoDbConfig.Instance.DatabaseId, "entreprises");
            FeedOptions feedOptions = new FeedOptions { MaxItemCount = -1 };
            IDocumentQuery<Entreprise> demandeurs = CosmoDbConfig.Instance.Client.
                    CreateDocumentQuery<Entreprise>(collectionUri, feedOptions)
                    .AsDocumentQuery();
            List<Entreprise> listofEntreprise = new List<Entreprise>();
            while (demandeurs.HasMoreResults)
                listofEntreprise.AddRange(demandeurs.ExecuteNextAsync<Entreprise>().Result);
            return listofEntreprise;
        }

        public IEnumerable<Entreprise> GetItems(Expression<Func<Entreprise, bool>> where)
        {

            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(CosmoDbConfig.Instance.DatabaseId, "entreprises");
            FeedOptions feedOptions = new FeedOptions { MaxItemCount = -1 };
            IDocumentQuery<Entreprise> demandeurs;
            if (where == null)
                demandeurs = CosmoDbConfig.Instance.Client.
                    CreateDocumentQuery<Entreprise>(collectionUri, feedOptions)
                    .AsDocumentQuery();
            else
                demandeurs = CosmoDbConfig.Instance.Client.
                    CreateDocumentQuery<Entreprise>(collectionUri, feedOptions)
                    .Where(where)
                    .AsDocumentQuery();
            List<Entreprise> listofEntreprise = new List<Entreprise>();
            while (demandeurs.HasMoreResults)
                listofEntreprise.AddRange(demandeurs.ExecuteNextAsync<Entreprise>().Result);
            return listofEntreprise;
        }

        public void NewItems(params Entreprise[] items)
        {
            foreach (var i in items)
                CosmoDbConfig.Instance
                    .Client
                    .CreateDocumentAsync(UriFactory
                    .CreateDocumentCollectionUri(CosmoDbConfig.Instance.DatabaseId, "entreprises"),
                    i);
        }

        public void UpdateItem(string id, Entreprise item)
        {
            if (string.IsNullOrEmpty(id.Trim()))
                throw new InvalidOperationException("Id is not specified");
            if (id != item.Id)
                throw new InvalidOperationException("Les id ne correspondent pas");
            Uri documentUri = UriFactory.CreateDocumentUri(CosmoDbConfig.Instance.DatabaseId, "entreprises", id);
            CosmoDbConfig.Instance
                    .Client
                    .ReplaceDocumentAsync(documentUri, item);
        }
    }
}
