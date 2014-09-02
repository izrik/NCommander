using NUnit.Framework;
using System;
using NCommander;
using System.Collections.Generic;

namespace NCommanderTests
{
    [TestFixture]
    public class CommandTest
    {
        [Test]
        public void CommandWithNoParametersShouldSucceedWhenGivenNoArguments()
        {
            // given
            var command = new Command();
            Dictionary<string, object> convertedArgs = null;
            command.ExecuteDelegate = (x) => convertedArgs = x;

            // when
            command.Execute(new string[0]);

            // then
            Assert.IsNotNull(convertedArgs);
            Assert.IsEmpty(convertedArgs);
        }

        [Test]
        public void CommandWithNoParametersShouldSucceedWhenGivenArguments()
        {
            // given
            var command = new Command();
            Dictionary<string, object> convertedArgs = null;
            command.ExecuteDelegate = (x) => convertedArgs = x;

            // when
            command.Execute(new string[] { "arg1", "arg2" });

            // then
            Assert.IsNotNull(convertedArgs);
            Assert.IsEmpty(convertedArgs);
        }

        [Test]
        public void CommandWithSingleParametersShouldSucceedWhenGivenOneArgument()
        {
            // given
            var command = new Command();
            command.Params = new Parameter[] {
                new Parameter { Name = "param", ParameterType = ParameterType.String }
            };
            Dictionary<string, object> convertedArgs = null;
            command.ExecuteDelegate = (x) => convertedArgs = x;

            // when
            command.Execute(new string[] { "arg1" });

            // then
            Assert.IsNotNull(convertedArgs);
            Assert.AreEqual(1, convertedArgs.Count);
            Assert.IsTrue(convertedArgs.ContainsKey("param"));
            Assert.AreEqual("arg1", convertedArgs["param"]);
        }

        [Test]
        public void CommandWithSingleParametersShouldSucceedWhenGivenTwoArguments()
        {
            // given
            var command = new Command();
            command.Params = new Parameter[] {
                new Parameter { Name = "param", ParameterType = ParameterType.String }
            };
            Dictionary<string, object> convertedArgs = null;
            command.ExecuteDelegate = (x) => convertedArgs = x;

            // when
            command.Execute(new string[] { "arg1", "arg2" });

            // then
            Assert.IsNotNull(convertedArgs);
            Assert.AreEqual(1, convertedArgs.Count);
            Assert.IsTrue(convertedArgs.ContainsKey("param"));
            Assert.AreEqual("arg1", convertedArgs["param"]);
        }

        [Test]
        public void CommandWithSingleParametersShouldFailWhenGivenNoArguments()
        {
            // given
            var command = new Command();
            command.Params = new Parameter[] {
                new Parameter { Name = "param", ParameterType = ParameterType.String }
            };
            Dictionary<string, object> convertedArgs = null;
            command.ExecuteDelegate = (x) => convertedArgs = x;

            // when
            var e = Assert.Throws<ArgumentException>(() => command.Execute(new string[0]));

            // then
            Assert.IsNotNull(e);
            Assert.IsTrue(e.Message.Contains("\"param\""));
            Assert.IsNull(convertedArgs);
        }

        [Test]
        public void OptionsStartWithTwoDashes()
        {
            // given
            var command = new Command();
            command.Options = new Option[] {
                new Option { Name = "option" }
            };
            Dictionary<string, object> convertedArgs = null;
            command.ExecuteDelegate = (x) => convertedArgs = x;

            // when
            command.Execute(new string[] { "--option" });

            // then
            Assert.IsNotNull(convertedArgs);
            Assert.AreEqual(1, convertedArgs.Count);
            Assert.IsTrue(convertedArgs.ContainsKey("option"));
            Assert.AreEqual(true, convertedArgs["option"]);
        }

        [Test]
        public void OptionsNotMatchedByAnyArgumentsGetFalse()
        {
            // given
            var command = new Command();
            command.Options = new Option[] {
                new Option { Name = "option" }
            };
            Dictionary<string, object> convertedArgs = null;
            command.ExecuteDelegate = (x) => convertedArgs = x;

            // when
            command.Execute(new string[] { "arg1" });

            // then
            Assert.IsNotNull(convertedArgs);
            Assert.AreEqual(1, convertedArgs.Count);
            Assert.IsTrue(convertedArgs.ContainsKey("option"));
            Assert.AreEqual(false, convertedArgs["option"]);
        }

        [Test]
        public void OptionsConsumeOneArgumentAndDontAffectParameters()
        {
            // given
            var command = new Command();
            command.Params = new Parameter[] {
                new Parameter { Name = "param", ParameterType = ParameterType.String }
            };
            command.Options = new Option[] {
                new Option { Name = "option" }
            };
            Dictionary<string, object> convertedArgs = null;
            command.ExecuteDelegate = (x) => convertedArgs = x;

            // when
            command.Execute(new string[] { "--option", "arg2" });

            // then
            Assert.IsNotNull(convertedArgs);
            Assert.AreEqual(2, convertedArgs.Count);
            Assert.IsTrue(convertedArgs.ContainsKey("option"));
            Assert.AreEqual(true, convertedArgs["option"]);
            Assert.IsTrue(convertedArgs.ContainsKey("param"));
            Assert.AreEqual("arg2", convertedArgs["param"]);
        }
    }
}

