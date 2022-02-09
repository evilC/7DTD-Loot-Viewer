using ConfigParsers.Blocks;
using ConfigParsers.Common;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class BlockParserTests
    {
        private BlocksParser _bp;
        private Count _oneCount = new Count("1,1");

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
            var block = _bp.Blocks["Test01-A"];
            block.Drops.Count.Should().Be(1);
            var drop = block.Drops[0];
            drop.ResourceName.Should().Be("DropOne");
            drop.Count.Equals(_oneCount).Should().BeTrue();
        }

        [Fact]
        public void Count_of_0_in_A_removes_inherited_drop_from_B()
        {
            // Extending block removes drops with count of 0
            var block = _bp.Blocks["Test02-A"];
            block.Drops.Count.Should().Be(0);
        }

        [Fact]
        public void Count_of_0_in_B_removes_drop_from_C()
        {
            // If count of 1 is set in A, and count of 0 is set in B, then count in C should be 0
            var block = _bp.Blocks["Test03-A"];
            block.Drops.Count.Should().Be(0);
        }
    }
}
