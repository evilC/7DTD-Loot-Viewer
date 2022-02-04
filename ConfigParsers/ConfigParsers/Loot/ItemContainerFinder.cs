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

        public Results GetItemProbabilities(string itemName)
        {
            var item = _data.Items[itemName];
            var paths = new List<Result>();
            for (int i = 0; i < item.Instances.Count; i++)
            {
                var thisPath = paths.Count();
                var instance = item.Instances[i];
                var result = new Result(instance);
                paths.Add(result);
                //Debug.WriteLine($"Instance {i}");
                GetParentGroups(instance.ParentGroup, null, paths, thisPath);
                //Debug.WriteLine($"End of chain\n");
            }

            //for (int i = 0; i < paths.Count(); i++)
            //{
            //    var str = "";
            //    foreach (var node in paths[i].Nodes)
            //    {
            //        str += $"{node.Group.Name} (c={node.Group.Count}, ri={node.GroupReferenceIndex}), ";
            //    }
            //    Debug.WriteLine($"PATH {i}: {str}");
            //}

            var results = new Results();
            for (int i = 0; i < paths.Count(); i++)
            {
                var result = paths[i];
                // Flip the order of the nodes, so that the list starts with the container
                result.Nodes.Reverse();
                var container = result.Nodes[0].Group.Name;
                ContainerResult containerResult;
                if (results.ContainerResults.ContainsKey(container))
                {
                    containerResult = results.ContainerResults[container];
                }
                else
                {
                    containerResult = new ContainerResult();
                    results.ContainerResults.Add(container, containerResult);
                }
                containerResult.Results.Add(result);
            }
            return results;
        }

        private void GetParentGroups(Group group, int? parentReferenceIndex, List<Result> paths, int currentIndex)
        {
            //Debug.WriteLine($"In group {group.Name}, adding to path {currentIndex}");
            paths[currentIndex].Nodes.Add(new Node(group, parentReferenceIndex));
            // If the path branches, process the other branches first...
            // ... so that we can clone the current path
            for (int i = 1; i < group.ParentGroupReferences.Count(); i++)
            {
                var groupReference = group.ParentGroupReferences[i];
                var oldResult = paths[currentIndex];
                paths.Add(new Result(oldResult.ItemInstance, new List<Node>(oldResult.Nodes)));
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

    public class Results
    {
        public Dictionary<string, ContainerResult> ContainerResults { get; set; } = new Dictionary<string, ContainerResult>();
    }

    public class ContainerResult
    {
        public List<Result> Results { get; } = new List<Result>();
    }

    public class Result
    {
        public ItemInstance ItemInstance { get; }

        public List<Node> Nodes { get; }

        public Result(ItemInstance itemInstance, List<Node>? nodes = null)
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

        public string Render()
        {
            var str = Group.Render();
            if (GroupReferenceIndex != null)
            {
                str += $" | GroupReferenceIndex={GroupReferenceIndex} | {Group.GroupReferences[(int)GroupReferenceIndex].Render()}";
            }
            return str;
        }
    }
}
