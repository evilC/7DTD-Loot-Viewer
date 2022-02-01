using LootParser;
using System;
using System.IO;
using Xunit;

namespace UnitTests
{
    public class UnitTest1 : IDisposable
    {
        public UnitTest1()
        {

        }

        public void Dispose()
        {
            
        }

        [Fact]
        public void Test1()
        {
            var path = Directory.GetCurrentDirectory();
            var parser = new ConfigParser(path);

        }
    }
}