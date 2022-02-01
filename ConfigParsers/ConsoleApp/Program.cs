// See https://aka.ms/new-console-template for more information

using ConfigParsers.Loot;

var loot = new LootParser(@"E:\Games\steamapps\common\7 Days To Die\Data\Config\loot.xml");
//var parser = new OldLootParser(@"E:\Games\steamapps\common\7 Days To Die\Data\Config");

var groupToolsTiered = loot.Data.Groups["groupToolsTiered"];
var groupRareToolsTiered = loot.Data.Groups["groupRareToolsTiered"]; // Should have 3 child groups: groupToolsTiered, groupToolsT2, groupToolsT3 and 1 parent: groupWorkbenchLoot04
var groupPassNGas = loot.Data.Groups["groupPassNGas"];
var groupPassNGas03 = loot.Data.Groups["groupPassNGas03"];
var passngasCrate = loot.Data.Containers["passngasCrate"];
var meleeToolSalvageT2Ratchet = loot.Data.Items["meleeToolSalvageT2Ratchet"];
var resourceMetalPipe = loot.Data.Items["resourceMetalPipe"];
Console.WriteLine("Done!");