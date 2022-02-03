// See https://aka.ms/new-console-template for more information

using ConfigParsers.Loot;
using System.Diagnostics;

//var lootXmlPath = @"E:\Games\steamapps\common\7 Days To Die\Data\Config\loot.xml";
var lootXmlPath = Path.Combine(new string[] { Directory.GetCurrentDirectory(), "SimpleLoot.xml" });
var loot = new LootParser(lootXmlPath);
//var oldLootParser = new OldLootParser(@"E:\Games\steamapps\common\7 Days To Die\Data\Config");

//var groupToolsTiered = loot.Data.Groups["groupToolsTiered"];
//var groupRareToolsTiered = loot.Data.Groups["groupRareToolsTiered"]; // Should have 3 child groups: groupToolsTiered, groupToolsT2, groupToolsT3 and 1 parent: groupWorkbenchLoot04
//var groupPassNGas = loot.Data.Groups["groupPassNGas"];
//var groupPassNGas03 = loot.Data.Groups["groupPassNGas03"];
//var passngasCrate = loot.Data.Containers["passngasCrate"];
//var meleeToolSalvageT2Ratchet = loot.Data.Items["meleeToolSalvageT2Ratchet"];
//var resourceMetalPipe = loot.Data.Items["resourceMetalPipe"];

var rw = new ItemContainerFinder(loot.Data);
var results = rw.GetItemProbabilities("targetitem");
//var results = rw.GetItemProbabilities("meleeToolSalvageT2Ratchet");

Debug.WriteLine($"RESULTS\n=======\n\n");
foreach (var containerResult in results.ContainerResults)
{
    Debug.WriteLine($"CONTAINER: {containerResult.Key}\n---------------------------------------");
    for (int nl = 0; nl < containerResult.Value.Results.Count; nl++)
    {
        var nodeList = containerResult.Value.Results[nl].Nodes;
        Debug.WriteLine($"PATH FOR RESULT #{nl}:\n");
        foreach (var node in nodeList)
        {
            var str = $"Group: {node.Group.Name}, Count: {node.Group.Count}";
            if (node.GroupReferenceIndex != null)
            {
                str += $" | GroupReference to next node - Index: {node.GroupReferenceIndex}, Count: {node.Group.GroupReferences[(int)node.GroupReferenceIndex].Count}";
            }
            Debug.WriteLine($"{str}");
        }
        Debug.WriteLine($"\n..........................\n");
    }
}

Console.WriteLine("Done!");