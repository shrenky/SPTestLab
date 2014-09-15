using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SharePoint;
using Microsoft.SharePoint.ApplicationPages.Calendar.Exchange;
using Microsoft.SharePoint.Utilities;

namespace SharePoint.TestLab.Query
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SPSite site = new SPSite("http://sam2012"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    //Test1(web);
                   // GetAllItemsIncludeFolders(web);
                    Test2(web);
                }

                Console.ReadKey();
            }

            List<string> l = new List<string>(){"1","2","3","4"};
            for (int i = 0; i < 3; i++)
            {
                string s = l.FirstOrDefault(k => k == 1.ToString());
                l.Remove(s);
                string s1 = l.FirstOrDefault(k => k == 2.ToString());
                l.Remove(s1);
            }

            foreach (string v in l)
            {
                Console.WriteLine(v);
            }

            Console.ReadKey();
        }

        private static void Test2(SPWeb web)
        {
            SPList list = web.Lists["Batch Delete Test"];
            SPQuery query = new SPQuery();
            query.Query = string.Concat("<Where>><Eq>", "<FieldRef Name = 'Title'/>", "<Value Type='Text'>1</Value>", "</Eq></Where>");
            SPListItemCollection items = list.GetItems(query);
            Console.WriteLine(items.Count);
            Console.ReadKey();
        }

        private static void GetAllItemsIncludeFolders(SPWeb web)
        {
            SPList list = web.Lists["Batch Delete Test"];
            SPQuery query = new SPQuery();
            query.Query = string.Format("<Query><OrderBy><FieldRef Name='ID' /></OrderBy></Query>");
            query.ViewFieldsOnly = true;
            query.ViewFields = "<FieldRef Name=\"ID\" />";
            query.ViewAttributes = "Scope=\"RecursiveAll\"";
            SPListItemCollection items = list.GetItems(query);
            List<string> itemIds = new List<string>();
            foreach (SPListItem item in items)
            {
                itemIds.Add(item.ID.ToString());
                Console.WriteLine(item.ID);
            }
        }

        private static void Test1(SPWeb web)
        {
            SPList list = web.Lists["Batch Delete Test"];
            List<string> itemsId = new List<string>() {"6", "7", "5"};
            SPFolder currentFolder = null;
            foreach (string id in itemsId)
            {
                int idInt = Int32.Parse(id);
                SPListItem tempItem = list.GetItemById(idInt);
                if (tempItem.Folder == null) //find the first item which is not a folder
                {
                    string folderUrl = SPUtility.GetUrlDirectory(tempItem.Url);
                    Console.WriteLine(string.Format("item id: {0}, folder url: {1}", id, folderUrl));
                    currentFolder = web.GetFolder(folderUrl);
                    Console.WriteLine("Got folder? " + (currentFolder == null));

                    if (currentFolder.UniqueId.Equals(list.RootFolder.UniqueId))
                    {
                        Console.WriteLine("Bo");
                        currentFolder = null;
                    }
                    break;
                }
                Console.WriteLine("!!");
            }

            Console.WriteLine("current folder is null? " + (currentFolder == null));
            StringBuilder queryStr = new StringBuilder("<Where><In><FieldRef Name='ID'/><Values>");
            foreach (string id in itemsId)
            {
                queryStr.AppendFormat("<Value Type='Integer'>{0}</Value>", id);
            }

            queryStr.Append("</Values></In></Where>");
            SPQuery query = new SPQuery() {Query = queryStr.ToString()};
            if (currentFolder != null)
            {
                query.Folder = currentFolder;
            }

            SPListItemCollection items = list.GetItems(query);
            foreach (SPListItem item in items)
            {
                Console.WriteLine(item.Title);
            }
        }

        public static SPFolder GetFolderByItem(SPListItem item)
        {
            SPFolder folder = null;
            if (item != null && item.Folder != null)
            {
                string folderUrl = SPUtility.GetUrlDirectory(item.Url);
                folder = item.ParentList.ParentWeb.GetFolder(folderUrl);
            }
            return folder;
        }
    }
}

