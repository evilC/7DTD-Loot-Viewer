using ConfigParsers.Loot.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigParsers.Loot
{
    public class ReverseWalker
    {
        private DataStore _data;
        public ReverseWalker(DataStore data)
        {
            _data = data;
        }

        public void GetItemProbabilities(string itemName)
        {
            var item = _data.Items[itemName];
            var paths = new List<List<string>>();
            for (int i = 0; i < item.Instances.Count; i++)
            {
                var thisPath = paths.Count();
                paths.Add(new List<string>());
                var instance = item.Instances[i];
                Debug.WriteLine($"Instance {i}");
                GetParentGroups(instance.ParentGroup, paths, thisPath);
                Debug.WriteLine($"End of chain\n");
            }

            for (int i = 0; i < paths.Count(); i++)
            {
                var str = "";
                foreach (var node in paths[i])
                {
                    str += node + ", ";
                }
                Debug.WriteLine($"PATH {i}: {str}");
            }
        }

        private void GetParentGroups(Group group, List<List<string>> paths, int currentPath)
        {
            Debug.WriteLine($"In group {group.Name}");
            paths[currentPath].Add(group.Name);
            //if (group.ParentGroupReferences.Count > 1)
            //{
            //    Debug.WriteLine($"{group.ParentGroupReferences.Count} paths exist");
            //}
            for (int i = 0; i < group.ParentGroupReferences.Count; i++)
            {
                var newPath = currentPath;
                if (i > 0)
                {
                    newPath = paths.Count();
                    paths.Add(new List<string>(paths[currentPath]));
                }
                var groupReference = group.ParentGroupReferences[i];
                GetParentGroups(groupReference.Parent, paths, newPath);
            }
        }

        /*
        private void GetParentGroups(Group parentGroup, ItemPath itemPath)
        {
            foreach (var groupReference in parentGroup.GroupReferences)
            {
                var node = new Node();
                node.GroupReference = groupReference;
                if (groupReference.Parent != null)
                {
                    node.Group = groupReference.Parent;
                    GetParentGroups(groupReference.Parent, itemPath);
                }
            }
            //var node = new Node();
            //node.GroupReference = parentGroup.ParentGroupReferences
        }
        */
    }

    public class ItemPaths
    {
        public List<ItemPath> Paths { get; } = new List<ItemPath>();
    }

    public class ItemPath
    {
        public ItemInstance ItemInstance { get; set; }
        public Group ContainingGroup { get; set; }
        public List<Node> Nodes { get; } = new List<Node>();
        //public Group Container { get; set; }
    }

    public class Node
    {
        public Group Group { get; set; }
        public GroupReference GroupReference { get; set; }
    }
}
