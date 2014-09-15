using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using System.Configuration;

namespace SharePoint.TestLab.TestThreshold
{
    class Program
    {
        static void Main(string[] args)
        {
            string siteUrl = ConfigurationManager.AppSettings["SiteUrl"];
            string webId = ConfigurationManager.AppSettings["WebId"];
            Guid webGuid = new Guid(webId);
            string listName = ConfigurationManager.AppSettings["ListName"];
            string administratorLoginName = ConfigurationManager.AppSettings["AdministratorLoginName"];
            string normalUserLoginName = ConfigurationManager.AppSettings["NormalUserLoginName"];
            SPUserToken administratorUserToken = Helper.User.UserPropertiesHelper.GetUserTokenByLoginName(siteUrl, webGuid, administratorLoginName);
            SPUserToken normalUserToken = Helper.User.UserPropertiesHelper.GetUserTokenByLoginName(siteUrl, webGuid, normalUserLoginName);

            Console.WriteLine("Get items by administrator");
            using (SPSite site = new SPSite(siteUrl, administratorUserToken))
            {
                using (SPWeb web = site.OpenWeb(webGuid))
                {
                    RunQuery(web, listName);
                }
            }

            Console.WriteLine("Get items by normal user");
            using (SPSite site = new SPSite(siteUrl, normalUserToken))
            {
                using (SPWeb web = site.OpenWeb(webGuid))
                {
                    RunQuery(web, listName);
                }
            }
            Console.ReadKey();
        }

        private static void RunQuery(SPWeb web, string listName)
        {
            SPList performanceList = web.Lists[listName];
            SPQuery query = new SPQuery();

            string queryString = String.Format(@"
                    <Where>
                        <Eq>
                            <FieldRef Name='City' />
                            <Value Type='Text'>City1</Value>
                        </Eq>
                    </Where>");
            query.Query = queryString;
            //query.ViewFields = string.Format("<FieldRef Name='{0}' />", "ID");
            //query.ViewFieldsOnly = true;
            query.RowLimit = 1000;
            //query.QueryThrottleMode = SPQueryThrottleOption.Override;

            SPListItemCollection listItems = performanceList.GetItems(query);

            Console.WriteLine(string.Format("Result: {0}", listItems.Count));
        }
    }
}
