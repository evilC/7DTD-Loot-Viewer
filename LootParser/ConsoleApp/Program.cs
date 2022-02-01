// See https://aka.ms/new-console-template for more information

using LootParser;

var parser = new ConfigParser(@"E:\Games\steamapps\common\7 Days To Die\Data\Config");
//var parser = new OldLootParser(@"E:\Games\steamapps\common\7 Days To Die\Data\Config");

var groupToolsTiered = parser.Loot.Groups["groupToolsTiered"];
var groupRareToolsTiered = parser.Loot.Groups["groupRareToolsTiered"]; // Should have 3 child groups: groupToolsTiered, groupToolsT2, groupToolsT3 and 1 parent: groupWorkbenchLoot04
var groupPassNGas = parser.Loot.Groups["groupPassNGas"];
var groupPassNGas03 = parser.Loot.Groups["groupPassNGas03"];
var passngasCrate = parser.Loot.Containers["passngasCrate"];
var meleeToolSalvageT2Ratchet = parser.Loot.Items["meleeToolSalvageT2Ratchet"];
var resourceMetalPipe = parser.Loot.Items["resourceMetalPipe"];
Console.WriteLine("Done!");