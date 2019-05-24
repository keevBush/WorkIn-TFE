using MvcControlsToolkit.Business.DocumentDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Services
{
    public class CosmoDbConfig
    {
        private static IDocumentDBConnection _instance = null;
        public static IDocumentDBConnection Instance
        {
            get
            {
                if(_instance==null)
                    _instance= new DefaultDocumentDBConnection("https://localhost:8081", 
                        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==", "workin");
                return _instance;
            }
        }
    }
}
