using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml;

namespace SharePoint.TestLab.ActiveDirectoryUtility.OrganizationalUnit
{
    public static class OuUtility
    {
        #region Construct OU tree by xml rapidly
        public static OuTreeNode GetOuTree()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement rootElement = null;
            string domainName = string.Empty;
            using (Domain domain = Domain.GetCurrentDomain())
            {
                rootElement = xmlDoc.CreateElement(domain.Name);
                xmlDoc.AppendChild(rootElement);

                using (DirectorySearcher ds = new DirectorySearcher(domain.GetDirectoryEntry(), "(objectClass=organizationalUnit)", null,
                    SearchScope.Subtree))
                {
                    SearchResultCollection src = ds.FindAll();;
                    
                    foreach (SearchResult result in src)
                    {
                        using (DirectoryEntry ouEntry = result.GetDirectoryEntry())
                        {
                            try
                            {
                                string nodeName = GetName(ouEntry);
                                string nodeId = GetId(ouEntry);
                                string xPathParent = GetParentPath(ouEntry, nodeName);
                                string xPath = GetPath(ouEntry);

                                if (xmlDoc.SelectSingleNode(xPath) == null)
                                {
                                    XmlElement newElement = xmlDoc.CreateElement(nodeName);
                                    newElement.SetAttribute("name", nodeName);
                                    newElement.SetAttribute("guid", nodeId);
                                    XmlNode parent = xmlDoc.SelectSingleNode(xPathParent);
                                    if (parent != null)
                                        parent.AppendChild(newElement);
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }

            OuTreeNode tree = BuildOuTree(xmlDoc);
            return tree;
        }

        private static OuTreeNode BuildOuTree(XmlDocument tempXml)
        {
            OuTreeNode root = new OuTreeNode();
            using (Domain domain = Domain.GetCurrentDomain())
            {
                root.Name = domain.Name;
            }
            GetOUTreeNode(root, tempXml.FirstChild);
            return root;
        }

        private static void GetOUTreeNode(OuTreeNode root, XmlNode tempXml)
        {
            foreach (XmlNode xmlNode in tempXml.ChildNodes)
            {
                OuTreeNode childNode = new OuTreeNode() { Name = XmlConvert.DecodeName(xmlNode.Attributes["name"].Value), Id = xmlNode.Attributes["guid"].Value };
                root.Children.Add(childNode);
                GetOUTreeNode(childNode, xmlNode);
            }
        }

        private static string GetName(DirectoryEntry entry)
        {
            return XmlConvert.EncodeLocalName(entry.Properties["name"].Value.ToString());
        }

        private static string GetId(DirectoryEntry entry)
        {
            byte[] bytes = entry.Properties["ObjectGuid"].Value as byte[];
            Guid id = new Guid(bytes);
            return id.ToString();
        }

        private static string GetParentPath(DirectoryEntry entry, string name)
        {
            string path = FormatDNToRelativePath(entry.Properties["distinguishedName"].Value.ToString());
            if (path.LastIndexOf(name) != -1)
            {
                path = path.Remove(path.LastIndexOf(name));
            }

            string parentPath = string.Format("/{0}", path);
            parentPath = parentPath.Substring(0, parentPath.LastIndexOf('/'));

            return parentPath;
        }

        private static string GetPath(DirectoryEntry entry)
        {
            string xPath = string.Format("/{0}", FormatDNToRelativePath(entry.Properties["distinguishedName"].Value.ToString()));
            return xPath;

        }

        private static string FormatDNToRelativePath(string rawDn)
        {
            StringBuilder path = new StringBuilder();
            rawDn = rawDn.Replace(" ", "_").Replace("&", "_");
            string[] dnParts = rawDn.Split(new[] { ',' });
            foreach (string part in dnParts)
            {
                if (part.StartsWith("OU", StringComparison.OrdinalIgnoreCase))
                {
                    string name = XmlConvert.EncodeName((part.Split(new char[] {'='})[1]));
                    path.Insert(0, string.Format("/{0}", name));
                }
            }
            string domainName = string.Empty;
            using (Domain root = Domain.GetCurrentDomain())
            {
                domainName = root.Name;
            }
            path.Insert(0, string.Format("{0}", domainName));
            return path.ToString();
        }

        #endregion

        #region Construct OU tree normal

        public static OuTreeNode GetOuTreeNormal()
        {
            string domainName = string.Empty;
            OuTreeNode rootNode = null;
            using (Domain domain = Domain.GetCurrentDomain())
            {
                domainName = domain.Name;
                rootNode = new OuTreeNode() { Name = domainName };
                GetOuTreeNormal(rootNode, domain.GetDirectoryEntry());
            }
            return rootNode;
        }

        private static void GetOuTreeNormal(OuTreeNode parentNode, DirectoryEntry parentDirectoryEntry)
        {
            using (DirectorySearcher ds = new DirectorySearcher(parentDirectoryEntry))
            {
                ds.Filter = "(objectClass=organizationalunit)";
                ds.SearchScope = SearchScope.OneLevel;

                try
                {
                    using (SearchResultCollection result = ds.FindAll())
                    {
                        foreach (SearchResult entry in result)
                        {
                            string name = entry.GetDirectoryEntry().Properties["Name"].Value.ToString();
                            byte[] bytes = entry.GetDirectoryEntry().Properties["ObjectGuid"].Value as byte[];
                            Guid id = new Guid(bytes);
                            OuTreeNode node = new OuTreeNode() { Name = name, Id = id.ToString() };
                            parentNode.Children.Add(node);
                            using (DirectoryEntry child = entry.GetDirectoryEntry())
                            {
                                GetOuTreeNormal(node, child);
                            }
                        }
                    }
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
        }

        #endregion

    }
}
