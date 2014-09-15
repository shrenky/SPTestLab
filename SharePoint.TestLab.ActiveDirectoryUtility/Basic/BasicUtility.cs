using System.DirectoryServices.ActiveDirectory;

namespace SharePoint.TestLab.ActiveDirectoryUtility.Basic
{
    public static class BasicUtility
    {
        public static Domain GetCurrentDomain(string domainName, string userName, string password)
        {
            return Domain.GetCurrentDomain();
        }
    }
}
