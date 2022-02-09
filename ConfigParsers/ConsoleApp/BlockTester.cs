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
                        bool found = false;
                        if (container.EndsWith("Master"))
                        {
                            var baseStr = container.Substring(0, container.Length - 6);
                            for (int i = 0; i < 10; i++)
                            {
                                var searchStr = $"{baseStr}v0{i}";
                                if (displayNames.ContainsKey(searchStr))
                                {
                                    Debug.WriteLine($"Found {searchStr}");
                                    found = true;
                                }
                            }
                        }
                        if (!found) Debug.WriteLine($"Could not find {container}");
                    }
                }
            }

        }
    }
}
