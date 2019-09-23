using MvcControlsToolkit.Business.DocumentDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Services
{
    public abstract class CosmoDbConfig
    {    
        private static IDocumentDBConnection _instance = null;
        public static IDocumentDBConnection Instance
        {
            get
            {
                if(_instance==null)
                    _instance = new DefaultDocumentDBConnection("https://tfebush12.documents.azure.com:443/",
                        "Fm5DY7GHRDXAC6ew2EWYXIGPBwt9IJY7PZ6MFaxmAWEitiJYzWqLYoyXxYiYatYakkZ5EwMgeALe7uZZbSJZAw==", "workin");
                return _instance;
            }
        }
    }
}
