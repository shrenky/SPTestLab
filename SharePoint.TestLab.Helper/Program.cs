using System;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.WebControls;
using SharePoint.TestLab.Helper.User;

namespace SharePoint.TestLab.Helper
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] sidBytes = UserPropertiesHelper.ConvertStringSidToSid("S-1-5-21-1343462910-744444023-3288617952-1638");
            string loginName = PeopleEditor.GetAccountFromSid(sidBytes);
            Console.WriteLine(loginName);
            Console.ReadKey();
        }
    }
}
