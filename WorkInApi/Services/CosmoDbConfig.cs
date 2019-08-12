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
                    _instance= new DefaultDocumentDBConnection("url",
                        "Key", "workin");
                return _instance;
            }
        }
    }
}
