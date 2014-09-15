using System;
using System.IO;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;

namespace SharePoint.TestLab.ClientOMConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SPSite site = new SPSite("http://localhost/sub"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList library = web.Lists["Documents"];
                    using (ClientContext ctx = new ClientContext(web.Url))
                    {
                        using (FileStream file = new FileStream(@"C:\users\a.txt", FileMode.Open))
                        {
                            string serverRelativeUrl = string.Format("{0}/{1}", library.RootFolder.ServerRelativeUrl, "b.txt");
                            Microsoft.SharePoint.Client.File.SaveBinaryDirect(ctx, serverRelativeUrl, file, true);
                        }
                    }
                }
            }
            Console.WriteLine("OK");
            Console.ReadKey();
        }
    }
}
