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
            var e = Assert.Throws<NotEnoughArgumentsForParameterException>(
                () => command.Execute(new string[0]));

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

        [Test]
        public void OptionsWithValuesConsumeTwoArgumentsAndDontAffectParameters()
        {
            // given
            var command = new Command();
            command.Params = new Parameter[] {
                new Parameter { Name = "param", ParameterType = ParameterType.String }
            };
            command.Options = new Option[] {
                new Option { Name = "option", Type=ParameterType.String }
            };
            Dictionary<string, object> convertedArgs = null;
            command.ExecuteDelegate = (x) => convertedArgs = x;

            // when
            command.Execute(new string[] { "--option", "arg2", "arg3" });

            // then
            Assert.IsNotNull(convertedArgs);
            Assert.AreEqual(2, convertedArgs.Count);
            Assert.IsTrue(convertedArgs.ContainsKey("option"));
            Assert.AreEqual("arg2", convertedArgs["option"]);
            Assert.IsTrue(convertedArgs.ContainsKey("param"));
            Assert.AreEqual("arg3", convertedArgs["param"]);
        }

        [Test]
        public void StringArrayConsumesAllArguments()
        {
            // given
            var command = new Command();
            command.Params = new [] {
                new Parameter { Name="param1", ParameterType=ParameterType.String },
                new Parameter { Name="array", ParameterType=ParameterType.StringArray },
            };
            Dictionary<string, object> convertedArgs = null;
            command.ExecuteDelegate = (x) => convertedArgs = x;

            // when
            command.Execute(new [] { "arg1", "arg2", "arg3" });

            // then
            Assert.IsNotNull(convertedArgs);
            Assert.AreEqual(2, convertedArgs.Count);
            Assert.IsTrue(convertedArgs.ContainsKey("param1"));
            Assert.AreEqual("arg1", convertedArgs["param1"]);
            Assert.IsTrue(convertedArgs.ContainsKey("array"));
            Assert.That(convertedArgs["array"] is string[]);
            var a = (string[])convertedArgs["array"];
            Assert.IsNotNull(a);
            Assert.AreEqual(2, a.Length);
            Assert.AreEqual("arg2", a[0]);
            Assert.AreEqual("arg3", a[1]);
        }

        // TODO: optional string array?

        // string array not last
        [Test]
        public void StringArrayMustBeTheLastParameter()
        {

            // given
            var command = new Command();
            command.Params = new [] {
                new Parameter { Name="array", ParameterType=ParameterType.StringArray },
                new Parameter { Name="param", ParameterType=ParameterType.String },
            };
            Dictionary<string, object> convertedArgs = null;
            command.ExecuteDelegate = (x) => convertedArgs = x;

            // when
            var e = Assert.Throws<StringArrayParameterOutOfPlaceException>(
                () => command.Execute(new string[]{"arg1", "arg2", "arg3"}));

            // then
            Assert.IsNotNull(e);
            Assert.IsTrue(e.Message.Contains("\"array\""));
            Assert.IsNull(convertedArgs);
        }

        [Test]
        public void OptionsCanGoBetweenArgsInAStringArray()
        {
            // given
            var command = new Command();
            command.Params = new [] {
                new Parameter { Name="array", ParameterType=ParameterType.StringArray },
            };
            command.Options = new [] {
                new Option { Name="opt", Type=ParameterType.String },
            };
            Dictionary<string, object> convertedArgs = null;
            command.ExecuteDelegate = (x) => convertedArgs = x;

            // when
            command.Execute(new [] { "arg1", "--opt", "value", "arg4" });

            // then
            Assert.IsNotNull(convertedArgs);
            Assert.AreEqual(2, convertedArgs.Count);
            Assert.IsTrue(convertedArgs.ContainsKey("opt"));
            Assert.AreEqual("value", convertedArgs["opt"]);
            Assert.IsTrue(convertedArgs.ContainsKey("array"));
            Assert.That(convertedArgs["array"] is string[]);
            var a = (string[])convertedArgs["array"];
            Assert.IsNotNull(a);
            Assert.AreEqual(2, a.Length);
            Assert.AreEqual("arg1", a[0]);
            Assert.AreEqual("arg4", a[1]);
        }

        [Test]
        public void OptionStringArrayCanBeUsedManyTimesConsumingOneArgEach()
        {
            // given
            var command = new Command
            {
                Options = new[]
                {
                    new Option
                    {
                        Name = "opt",
                        Type = ParameterType.StringArray
                    },
                }
            };
            Dictionary<string, object> convertedArgs = null;
            command.ExecuteDelegate = (x) => convertedArgs = x;

            // when
            command.Execute(new[] {"--opt", "value1", "--opt", "value2"});

            // then
            Assert.IsNotNull(convertedArgs);
            Assert.AreEqual(1, convertedArgs.Count);
            Assert.IsTrue(convertedArgs.ContainsKey("opt"));
            Assert.IsInstanceOf<string[]>(convertedArgs["opt"]);
            Assert.AreEqual(new string[] {"value1", "value2"},
                convertedArgs["opt"]);
        }
    }
}

