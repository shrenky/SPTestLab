using System;
using System.Diagnostics;
using System.Text;
using SharePoint.TestLab.ActiveDirectoryUtility.OrganizationalUnit;

namespace SharePoint.TestLab.ActiveDirectoryUtility
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Stopwatch watcher = new Stopwatch();
            watcher.Start();
            OuTreeNode tree = OrganizationalUnit.OuUtility.GetOuTree();
            watcher.Stop();
            Console.WriteLine(watcher.ElapsedMilliseconds);
            print(tree, 1);
            watcher.Reset();
            watcher.Start();
            tree = OrganizationalUnit.OuUtility.GetOuTreeNormal();
            watcher.Stop();
            Console.WriteLine(watcher.ElapsedMilliseconds);
            print(tree, 1);
            Console.ReadKey();

        }

        private static void print(OuTreeNode tree, int depth)
        {
            depth += 1;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < depth; i++)
            {
                builder.Append("  ");
            }
            builder.Append(tree.Name);
            Console.WriteLine(builder.ToString());
            if (tree.Children.Count > 0)
            {
                foreach (OuTreeNode node in tree.Children)
                {
                    print(node, depth);
                }
            }
        }
    }
}