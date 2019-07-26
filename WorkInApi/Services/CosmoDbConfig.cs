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
                    _instance= new DefaultDocumentDBConnection("https://bushffe.documents.azure.com:443/",
                        "qGwmco6RolIxOEoKw8UQYtn4e7gznkt9KWIN60Bv5zFAaLBPc2VFrPcud1Oq1or2qxj27wlvjeZPRLnLefSzcA==", "workin");
                return _instance;
            }
        }
    }
}
