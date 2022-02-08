using ConfigParsers.Blocks;
using ConfigParsers.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            bp.LoadConfigFile(configFilePath);
            var lp = new LocalizationParser();
            var displayNames = lp.GetDisplayNames(configFilePath);
            foreach (var list in bp.BlockList)
            {
                foreach (var container in list.Value)
                {
                    if (!displayNames.ContainsKey(container))
                    {
                        Debug.WriteLine($"Could not find {container}");
                    }
                }
            }

        }
    }
}
