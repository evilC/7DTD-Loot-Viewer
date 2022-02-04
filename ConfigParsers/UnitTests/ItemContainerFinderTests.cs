using ConfigParsers.Loot;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class ItemContainerFinderTests
    {
        private LootParser _lootParser;
        private ItemContainerFinder _itemContainerFinder;
        private ItemContainers _itemContainers;

        public ItemContainerFinderTests()
        {
            var lootXmlPath = Path.Combine(new string[] { Directory.GetCurrentDirectory(), "ItemContainerFinderTests.xml" });
            _lootParser = new LootParser(lootXmlPath);

            _itemContainerFinder = new ItemContainerFinder(_lootParser.Data);
            _itemContainers = _itemContainerFinder.GetItemContainers("targetitem");
        }

        [Fact]
        public void Correct_containers_should_be_found()
        {
            _itemContainers.ContainerResults.Count.Should().Be(2);
            _itemContainers.ContainerResults.Should().ContainKeys("container-a1", "container-a2");
        }

        [Fact]
        public void Container_a1_should_have_correct_number_of_paths()
        {
            var a1 = _itemContainers.ContainerResults["container-a1"];
            a1.Paths.Count.Should().Be(3);
        }

        [Fact]
        public void Container_a1_should_have_correct_nodes()
        {
            var a1 = _itemContainers.ContainerResults["container-a1"];
            var r0 = a1.Paths[0];
            r0.Nodes.Count.Should().Be(4);
            r0.Nodes[0].Group.Name.Should().Be("container-a1");
            r0.Nodes[1].Group.Name.Should().Be("group-b1");
            r0.Nodes[2].Group.Name.Should().Be("group-c1");
            r0.Nodes[3].Group.Name.Should().Be("group-d1");

            var r1 = a1.Paths[1];
            r1.Nodes.Count.Should().Be(3);
            r1.Nodes[0].Group.Name.Should().Be("container-a1");
            r1.Nodes[1].Group.Name.Should().Be("group-b1");
            r1.Nodes[2].Group.Name.Should().Be("group-d1");

            var r2 = a1.Paths[2];
            r2.Nodes.Count.Should().Be(2);
            r2.Nodes[0].Group.Name.Should().Be("container-a1");
            r2.Nodes[1].Group.Name.Should().Be("group-b1");
        }

        [Fact]
        public void Container_a2_should_have_correct_number_of_paths()
        {
            var a2 = _itemContainers.ContainerResults["container-a2"];
            a2.Paths.Count.Should().Be(1);
        }

        [Fact]
        public void Container_a2_should_have_correct_nodes()
        {
            var a2 = _itemContainers.ContainerResults["container-a2"];
            var r0 = a2.Paths[0];
            r0.Nodes.Count.Should().Be(3);
            r0.Nodes[0].Group.Name.Should().Be("container-a2");
            r0.Nodes[1].Group.Name.Should().Be("group-c1");
            r0.Nodes[2].Group.Name.Should().Be("group-d1");
        }
    }
}
