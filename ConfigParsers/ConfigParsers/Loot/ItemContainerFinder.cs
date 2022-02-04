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

        public ItemContainers GetItemContainers(string itemName)
        {
            var item = _data.Items[itemName];
            var itemPaths = new List<ItemPath>();
            for (int i = 0; i < item.Instances.Count; i++)
            {
                var thisPath = itemPaths.Count();
                var instance = item.Instances[i];
                var result = new ItemPath(instance);
                itemPaths.Add(result);
                //Debug.WriteLine($"Instance {i}");
                GetParentGroups(instance.ParentGroup, null, itemPaths, thisPath);
                //Debug.WriteLine($"End of chain\n");
            }

            var itemContainers = new ItemContainers();
            for (int i = 0; i < itemPaths.Count(); i++)
            {
                var itemPath = itemPaths[i];
                // Flip the order of the nodes, so that the list starts with the container
                itemPath.Nodes.Reverse();
                var containerName = itemPath.Nodes[0].Group.Name;
                ItemContainer itemContainer;
                if (itemContainers.ContainerResults.ContainsKey(containerName))
                {
                    itemContainer = itemContainers.ContainerResults[containerName];
                }
                else
                {
                    itemContainer = new ItemContainer();
                    itemContainers.ContainerResults.Add(containerName, itemContainer);
                }
                itemContainer.Paths.Add(itemPath);
            }
            return itemContainers;
        }

        private void GetParentGroups(Group group, int? parentReferenceIndex, List<ItemPath> paths, int currentIndex)
        {
            //Debug.WriteLine($"In group {group.Name}, adding to path {currentIndex}");
            paths[currentIndex].Nodes.Add(new Node(group, parentReferenceIndex));
            // If the path branches, process the other branches first...
            // ... so that we can clone the current path
            for (int i = 1; i < group.ParentGroupReferences.Count(); i++)
            {
                var groupReference = group.ParentGroupReferences[i];
                var oldResult = paths[currentIndex];
                paths.Add(new ItemPath(oldResult.ItemInstance, new List<Node>(oldResult.Nodes)));
                GetParentGroups(groupReference.Parent, groupReference.ParentGroupReferenceIndex, paths, paths.Count()-1);
            }

            // Process the current branch
            if (group.ParentGroupReferences.Count != 0)
            {
                var groupReference = group.ParentGroupReferences[0];
                GetParentGroups(groupReference.Parent, groupReference.ParentGroupReferenceIndex, paths, currentIndex);
            }
        }
    }

    public class ItemContainers
    {
        public Dictionary<string, ItemContainer> ContainerResults { get; set; } = new Dictionary<string, ItemContainer>();
    }

    public class ItemContainer
    {
        public List<ItemPath> Paths { get; } = new List<ItemPath>();
    }

    public class ItemPath
    {
        public ItemInstance ItemInstance { get; }

        public List<Node> Nodes { get; }

        public ItemPath(ItemInstance itemInstance, List<Node>? nodes = null)
        {
            if (nodes == null) nodes = new List<Node>();
            ItemInstance = itemInstance;
            Nodes = nodes;
        }


    }

    public class Node
    {
        public Group Group { get; }
        public int? GroupReferenceIndex { get; }

        public Node(Group group, int? groupReferenceIndex = null)
        {
            Group = group;
            GroupReferenceIndex = groupReferenceIndex;
        }

        public GroupReference? GetGroupReference()
        {
            if (GroupReferenceIndex != null)
            {
                return Group.GroupReferences[(int)GroupReferenceIndex];
            }
            return null;
        }

        public decimal GetProb(int lootLevel)
        {
            var gr = GetGroupReference();
            if (gr == null) return 1;
            return gr.GetProb(lootLevel);
        }

        public string Render()
        {
            var str = Group.Render();
            var gr = GetGroupReference();
            if (gr != null)
            {
                str += $" | GroupReferenceIndex={GroupReferenceIndex} | {gr.Render()}";
            }
            return str;
        }
    }
}
