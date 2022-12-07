using System;
using System.IO;
using System.Text;
using NCommander;
using NUnit.Framework;

namespace NCommanderTests.LineReaderT
{
    [TestFixture]
    public class LineReaderTest
    {
        [Test]
        public void ReadsLine()
        {
            // given
            Console.SetIn(new StringReader("this is input"));
            // when
            var result = LineReader.ReadLine("> ");
            // then
            Assert.AreEqual("this is input", result);
        }

        [Test]
        public void OnlyFirstLineIsRead()
        {
            // given
            Console.SetIn(new StringReader("this is input\nhere is more"));
            // when
            var result = LineReader.ReadLine("> ");
            // then
            Assert.AreEqual("this is input", result);
        }

        [Test]
        public void SubsequentCallsReadSubsequentLines()
        {
            // given
            Console.SetIn(new StringReader("this is input\nhere is more"));
            var first = LineReader.ReadLine("> ");
            // precondition
            Assert.AreEqual("this is input", first);
            // when
            var result = LineReader.ReadLine("> ");
            // then
            Assert.AreEqual("here is more", result);
        }
        
        [Test]
        public void EmptyStringYieldsEmptyString()
        {
            // given
            Console.SetIn(new StringReader(""));
            // when
            var result = LineReader.ReadLine("> ");
            // then
            Assert.IsNull(result);
        }
        
        [Test]
        public void NewlineYieldsNull()
        {
            // given
            Console.SetIn(new StringReader("\n"));
            // when
            var result = LineReader.ReadLine("> ");
            // then
            Assert.AreEqual("", result);
        }
        
        [Test]
        public void WritesPromptToOutput()
        {
            // given
            Console.SetIn(new StringReader("one two three\n"));
            var sb = new StringBuilder();
            Console.SetOut(new StringWriter(sb));
            // when
            var result = LineReader.ReadLine("> ");
            // then
            Assert.AreEqual("> ", sb.ToString());
            Assert.AreEqual("one two three", result);
        }
    }
}