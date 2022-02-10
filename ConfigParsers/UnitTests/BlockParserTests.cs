﻿using ConfigParsers.Blocks;
using ConfigParsers.Common;
using FluentAssertions;
using Xunit;

namespace UnitTests
{
    public class BlockParserTests
    {
        private BlocksParser _bp;
        private Count _oneCount = new Count("1,1");
        private Count _zeroCount = new Count("0,0");

        // In all tests, "A" is the leaf block
        // ie A inherits from B, which inherits from C, etc
        public BlockParserTests()
        {
            _bp = new BlocksParser();
            _bp.LoadConfigFile("BlockParserTests.xml");
        }

        [Fact]
        public void A_inherits_from_B()
        {
            // Extending block inherits drops from things it extends
            var block = _bp.Blocks["FirstLevelAdd-A"];
            block.Drops.Count.Should().Be(1);
            var drop = block.Drops[0];
            drop.ResourceName.Should().Be("DropOne");
            drop.Count.Equals(_oneCount).Should().BeTrue();
        }

        [Fact]
        public void B_adds_to_C_and_A_inherits_from_B()
        {
            var block = _bp.Blocks["SecondLevelAdd-A"];
            block.Drops.Count.Should().Be(1);
            var drop = block.Drops[0];
            drop.ResourceName.Should().Be("DropOne");
            drop.Count.Equals(_oneCount).Should().BeTrue();
        }

        [Fact]
        public void Count_of_0_in_A_removes_inherited_drop_from_B()
        {
            // Extending block removes drops with count of 0
            var block = _bp.Blocks["FirstLevelRemove-A"];
            block.Drops.Count.Should().Be(0);
        }

        [Fact]
        public void Count_of_0_in_B_removes_drop_from_C()
        {
            // If count of 1 is set in A, and count of 0 is set in B, then count in C should be 0
            var block = _bp.Blocks["SecondLevelRemove-A"];
            block.Drops.Count.Should().Be(0);
        }

        [Fact]
        public void LootList_of_A_replaces_LootList_of_B()
        {
            // If extending Block and extended Block both have a LootList...
            // ... Then extending Block overwrites extended Block's LootList
            var block = _bp.Blocks["LootListInheritReplace-A"];
            block.LootList.Should().Be("LootList-A");
        }

        [Fact]
        public void Extending_block_with_no_LootList_does_not_replace_extended_blocks_LootList()
        {
            // If extending Block has a LootList but extended Block does not...
            // ... Then extending Block should not overwrite extended Block's LootList
            var block = _bp.Blocks["LootListInheritNoReplace-A"];
            block.LootList.Should().Be("LootList-B");
        }

    }
}