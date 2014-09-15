using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;
using SharePoint.TestLab.Helper.User;

namespace SharePoint.TestLab.AnonymousUser
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SPSite site = new SPSite("http://sam2012:33333"))
            {
                using (SPWeb web = site.OpenWeb("/sub"))
                {
                    web.AnonymousPermMask64 |= SPBasePermissions.UseRemoteAPIs;//SPBasePermissions.OpenItems;
                    Console.Write(web.AnonymousPermMask64);
                    Console.Write(web.DoesUserHavePermissions(SPBasePermissions.AddListItems));
                    SPList list = web.Lists["Temp"];
                    bool addItemPerm = list.DoesUserHavePermissions(SPBasePermissions.AddListItems);
                    Console.WriteLine(addItemPerm);
                    Console.WriteLine(list.AnonymousPermMask64);
                    web.Update();
                    Console.ReadKey();
                }
            }

            //byte[] sidBytes = UserPropertiesHelper.ConvertStringSidToSid("S-1-5-21-1343462910-744444023-3288617952-1638");
            //string loginName = PeopleEditor.GetAccountFromSid(sidBytes);
            //Console.WriteLine(loginName);
            //Console.ReadKey();

            //foreach (SPServer server in SPFarm.Local.Servers)
            //{
            //    Console.WriteLine(server.Name + "--" + server.Role);
            //    //Helper.Administration.Server.IsWebFrontEnd(server);
            //}
            //Console.ReadKey();
        }
    }
}
