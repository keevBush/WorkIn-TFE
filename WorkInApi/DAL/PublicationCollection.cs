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
    public class PublicationCollection : Services.IDataAccess<Publication>
    {
        public void DeleteItem(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Publication> GetAllItem()
        {
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(CosmoDbConfig.Instance.DatabaseId, "publications");
            FeedOptions feedOptions = new FeedOptions { MaxItemCount = -1 };
            IDocumentQuery<Publication> publications = CosmoDbConfig.Instance.Client.
                    CreateDocumentQuery<Publication>(collectionUri, feedOptions)
                    .AsDocumentQuery();
            List<Publication> listofPublications = new List<Publication>();
            while (publications.HasMoreResults)
                listofPublications.AddRange(publications.ExecuteNextAsync<Publication>().Result);
            return listofPublications;
        }

        public IEnumerable<Publication> GetItems(Expression<Func<Publication, bool>> where)
        {
            throw new NotImplementedException();
        }

        public void NewItems(params Publication[] items)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(string id, Publication item)
        {
            throw new NotImplementedException();
        }
    }
}
