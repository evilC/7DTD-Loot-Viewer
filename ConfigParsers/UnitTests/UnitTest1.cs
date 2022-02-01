using ConfigParsers.Loot;
using System.IO;
using Xunit;
using FluentAssertions;

namespace UnitTests
{
    public class UnitTest1
    {
        private LootParser _lootParser;
        public UnitTest1()
        {
            var lootXmlPath = Path.Combine(new string[] { Directory.GetCurrentDirectory(), "loot.xml" });
            _lootParser = new LootParser(lootXmlPath);
        }

        [Fact]
        public void Items_contains_target_item()
        {
            var targetitem = _lootParser.Data.Items["targetitem"];
            _lootParser.Data.Items.Should().ContainKey("targetitem");
        }
    }
}