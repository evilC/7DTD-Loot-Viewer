// See https://aka.ms/new-console-template for more information

using ConfigParsers.Blocks;
using ConfigParsers.Localization;
using ConfigParsers.Loot;
using System.Diagnostics;

var configFilePath = @"E:\Games\steamapps\common\7 Days To Die\Data\Config";

var lp = new LocalizationParser();
var displayNames = lp.GetDisplayNames(configFilePath);

var bp = new BlocksParser(displayNames);
var lootList = bp.GetLootLists(configFilePath);

var gameName = "cupboard";
Debug.WriteLine($"Game Name : {gameName}");
var containers = lootList[gameName];
foreach (var container in containers)
{
    Debug.WriteLine($"Display Name: {container}");
}
return;
var lootXmlPath = @"E:\Games\steamapps\common\7 Days To Die\Data\Config\loot.xml";
//var lootXmlPath = Path.Combine(new string[] { Directory.GetCurrentDirectory(), "Ratchet.xml" });
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
//var results = rw.GetItemContainers("meleeToolSalvageT2Ratchet");
var results = rw.GetItemContainers("resourceSewingKit");

//foreach (var containerResult in results.ContainerResults)
//{
//    Debug.WriteLine($"---------------------------------------\nCONTAINER: {containerResult.Key}\n");
//    for (int nl = 0; nl < containerResult.Value.Results.Count; nl++)
//    {
//        var nodeList = containerResult.Value.Results[nl].Nodes;
//        Debug.WriteLine($"PATH FOR RESULT #{nl}:");
//        foreach (var node in nodeList)
//        {
//            var str = node.Render();
//            Debug.WriteLine($"{str}");
//        }
//        Debug.WriteLine($"");
//    }
//}

//var container = results.ContainerResults.FirstOrDefault();
foreach (var container in results.ContainerResults)
{
    Debug.WriteLine($"Showing probabilities for container {container.Key}");
    var cr = container.Value;
    var probCalc = new ProbabilityCalculator(cr);
    probCalc.DebugMode = true;

    var prob1 = probCalc.CalculateProbability(50);
    Debug.WriteLine($"Total Probability: {prob1}");
    Debug.WriteLine($"==============================\n");

    var prob102 = probCalc.CalculateProbability(102);
    Debug.WriteLine($"Total Probability: {prob102}");
    Debug.WriteLine($"==============================\n");
}
//var prob134 = probCalc.CalculateProbability(134);
//Debug.WriteLine($"Total Probability: {prob134}");



Console.WriteLine("Done!");