using System.Collections.Generic;

namespace SharePoint.TestLab.ActiveDirectoryUtility.OrganizationalUnit
{
    public class OuTreeNode
    {
        public string Name { get; set; }
        public string Id { get; set; }
        private List<OuTreeNode> _children = new List<OuTreeNode>();

        public List<OuTreeNode> Children
        {
            get { return _children; }
            set { _children = value; }
        }
    }
}
