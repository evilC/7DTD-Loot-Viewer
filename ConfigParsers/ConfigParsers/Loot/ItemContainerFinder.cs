using ConfigParsers.Loot.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigParsers.Loot
{
    /// <summary>
    /// For a given Item, finds which Contaainer (And Groups) that it is in
    /// </summary>
    public class ItemContainerFinder
    {
        private DataStore _data;
        public ItemContainerFinder(DataStore data)
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
                //Debug.WriteLine($"Instance {i}");
                GetParentGroups(instance.ParentGroup, paths, thisPath);
                //Debug.WriteLine($"End of chain\n");
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

        private void GetParentGroups(Group group, List<List<string>> paths, int currentIndex)
        {
            //Debug.WriteLine($"In group {group.Name}, adding to path {currentIndex}");
            paths[currentIndex].Add(group.Name);
            // If the path branches, process the other branches first...
            // ... so that we can clone the current path
            for (int i = 1; i < group.ParentGroupReferences.Count(); i++)
            {
                var groupReference = group.ParentGroupReferences[i];
                paths.Add(new List<string>(paths[currentIndex]));
                GetParentGroups(groupReference.Parent, paths, paths.Count()-1);
            }

            // Process the current branch
            if (group.ParentGroupReferences.Count != 0)
            {
                GetParentGroups(group.ParentGroupReferences[0].Parent, paths, currentIndex);
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
