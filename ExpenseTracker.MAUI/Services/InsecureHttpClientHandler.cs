using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MAUI.Services
{
    public class InsecureHttpClientHandler : HttpClientHandler
    {
        public InsecureHttpClientHandler()
        {
            //Allows HTTP and ignore self-signed certificate in debug mode
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        }
    }
}
