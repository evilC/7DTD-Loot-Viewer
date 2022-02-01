using ConfigParsers.Loot;
using System.IO;
using Xunit;
using FluentAssertions;

namespace UnitTests
{
    public class LootParserTests
    {
        private LootParser _lootParser;
        public LootParserTests()
        {
            var lootXmlPath = Path.Combine(new string[] { Directory.GetCurrentDirectory(), "loot.xml" });
            _lootParser = new LootParser(lootXmlPath);
        }

        [Fact]
        public void Items_contains_target_item()
        {
            _lootParser.Data.Items.Should().ContainKey("targetitem");
        }

        [Fact]
        public void Target_item_has_two_instances()
        {
            var targetitem = _lootParser.Data.Items["targetitem"];
            targetitem.Instances.Count.Should().Be(2);
        }

        [Fact]
        public void Target_item_has_correct_parents()
        {
            var targetitem = _lootParser.Data.Items["targetitem"];
            targetitem.Instances[0].ParentGroup.Name.Should().Be("group-04-01");
            targetitem.Instances[1].ParentGroup.Name.Should().Be("group-03-02");
        }

        [Fact]
        public void Group_03_02_should_have_target_item_as_child()
        {
            _lootParser.Data.Groups["group-03-02"].Items.Should().ContainKey("targetitem");
        }

        [Fact]
        public void Group_04_01_should_have_target_item_as_child()
        {
            _lootParser.Data.Groups["group-04-01"].Items.Should().ContainKey("targetitem");
        }
    }
}