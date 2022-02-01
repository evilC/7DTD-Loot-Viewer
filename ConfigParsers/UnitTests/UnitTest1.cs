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
            var lootParser = new LootParser(Directory.GetCurrentDirectory());
        }
    }
}