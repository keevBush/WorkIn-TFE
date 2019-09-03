using Microsoft.Azure.Documents.Client;
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
            throw new NotImplementedException();
        }

        public IEnumerable<Proposition> GetItems(Expression<Func<Proposition, bool>> where)
        {
            throw new NotImplementedException();
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
