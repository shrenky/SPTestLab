using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Administration;

namespace SharePoint.TestLab.Helper.Administration
{
    public static class Server
    {
        public static bool IsWebFrontEnd(SPServer server)
        {
            bool isWFE = false;
            foreach (SPServiceInstance spServiceInstance in server.ServiceInstances)
            {
                if (spServiceInstance is SPWebServiceInstance && spServiceInstance.Status != SPObjectStatus.Disabled && spServiceInstance.Name != "WSS_Administration")
                {
                    isWFE = true;
                    break;
                }
            }
            return isWFE;
        }
    }
}
