using ConfigParsers.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class LocalizationTester
    {
        public LocalizationTester()
        {
            var configFilePath = @"E:\Games\steamapps\common\7 Days To Die\Data\Config";
            var lp = new LocalizationParser();
            var displayNames = lp.GetDisplayNames(configFilePath);
        }
    }
}
