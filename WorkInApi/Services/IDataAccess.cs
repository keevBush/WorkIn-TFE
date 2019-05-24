using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WorkInApi.Services
{
    public interface IDataAccess<T> where T: class
    {
        IEnumerable<T> GetAllItem();
        void NewItems(params T[] items);
        void UpdateItem(string id, T item);
        void DeleteItem(string id);
        IEnumerable<T> GetItems(Expression<Func<T, bool>> where);
        
    }
}
