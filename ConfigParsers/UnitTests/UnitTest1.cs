using ConfigParsers.Loot;
using System.IO;
using Xunit;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var lootXmlPath = Path.Combine(new string[] { Directory.GetCurrentDirectory(), "loot.xml" });
            var lootParser = new LootParser(lootXmlPath);
        }
    }
}