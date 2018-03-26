using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PageObjectModelGenerator.CLI.Utils.Tests
{
    [TestClass]
    public class ConsoleAttributeParserTests
    {
        [TestMethod]
        public void ParseArgs_ValidArgs()
        {
            // Arrange
            var args = new[] { "-p", "someprocess.exe", "-c", "ControlName" };

            // Act
            var dict = new ConsoleAttributeParser().ParseArgs(args);

            // Assert
            Assert.AreEqual(dict.Count, 2);
            Assert.IsTrue(dict.ContainsKey("-p"));
            Assert.IsTrue(dict.ContainsKey("-c"));
            Assert.AreEqual(dict["-p"], "someprocess.exe");
            Assert.AreEqual(dict["-c"], "ControlName");
        }

        [TestMethod]
        public void ParseArgs_OneParamIsMissing()
        {
            // Arrange
            var args = new[] { "-p", "-c", "ControlName" };

            // Act
            var dict = new ConsoleAttributeParser().ParseArgs(args);

            // Assert
            Assert.AreEqual(dict.Count, 2);
            Assert.IsTrue(dict.ContainsKey("-p"));
            Assert.IsTrue(dict.ContainsKey("-c"));
            Assert.AreEqual(dict["-p"], string.Empty);
            Assert.AreEqual(dict["-c"], "ControlName");
        }

        [TestMethod]
        public void ParseArgs_LastParamIsMissing()
        {
            // Arrange
            var args = new[] { "-p", "someprocess.exe", "-c"};

            // Act
            var dict = new ConsoleAttributeParser().ParseArgs(args);

            // Assert
            Assert.AreEqual(dict.Count, 2);
            Assert.IsTrue(dict.ContainsKey("-p"));
            Assert.IsTrue(dict.ContainsKey("-c"));
            Assert.AreEqual(dict["-p"], "someprocess.exe");
            Assert.AreEqual(dict["-c"], string.Empty);
        }

        [TestMethod]
        public void ParseArgs_InvalidLastArgs()
        {
            // Arrange
            var args = new[] { "-p", "someprocess.exe", "-c", "ControlName", "-abc", "that-param-doesnt-match-pattern" };

            // Act
            var dict = new ConsoleAttributeParser().ParseArgs(args);

            // Assert
            Assert.AreEqual(dict.Count, 2);
            Assert.IsTrue(dict.ContainsKey("-p"));
            Assert.IsTrue(dict.ContainsKey("-c"));
            Assert.AreEqual(dict["-p"], "someprocess.exe");
            Assert.AreEqual(dict["-c"], "ControlName");
        }

        [TestMethod]
        public void ParseArgs_NoArgs()
        {
            // Arrange
            var args = new string[] { };

            // Act
            var dict = new ConsoleAttributeParser().ParseArgs(args);

            // Assert
            Assert.AreEqual(dict.Count, 0);
        }
    }
}