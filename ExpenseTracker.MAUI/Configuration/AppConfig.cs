using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.MAUI.Configuration
{
    public class AppConfig
    {
#if DEBUG
        //Development
        public const string BaseUrl = "https://10.0.2.2:7050/api/";
#else
        //Production
        public const string BaseUrl = "https://192.168.0.197:5000/api/";
#endif
    }
}
