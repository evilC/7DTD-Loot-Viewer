using ConfigParsers.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class BlockTester
    {
        public BlockTester()
        {
            var configFilePath = @"E:\Games\steamapps\common\7 Days To Die\Data\Config";
            var bp = new BlocksParser();
            var lootList = bp.GetLootLists(configFilePath);
        }
    }
}
