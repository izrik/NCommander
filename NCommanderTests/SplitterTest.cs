using System;
using NUnit.Framework;
using NCommander;

namespace NCommanderTests
{
    [TestFixture]
    public class SplitterTest
    {
        [Test]
        public void TestBasicFunction()
        {
            // given
            var input = "arg1 arg2 'quoted arg with spaces' \"dquoted arg\" with.dot last-one";

            // when
            var args = Splitter.SplitArgs(input);

            // then
            Assert.IsNotNull(args);
            Assert.AreEqual(6, args.Length);
            Assert.AreEqual("arg1", args[0]);
            Assert.AreEqual("arg2", args[1]);
            Assert.AreEqual("quoted arg with spaces", args[2]);
            Assert.AreEqual("dquoted arg", args[3]);
            Assert.AreEqual("with.dot", args[4]);
            Assert.AreEqual("last-one", args[5]);
        }

        [Test]
        public void TestQuotesWithinQuotes()
        {
            //given
            string[] args;

            // when
            args = Splitter.SplitArgs("arg1 'dquote \" within quote' arg3");

            // then
            Assert.IsNotNull(args);
            Assert.AreEqual(3, args.Length);
            Assert.AreEqual("arg1", args[0]);
            Assert.AreEqual("dquote \" within quote", args[1]);
            Assert.AreEqual("arg3", args[2]);

            // when
            args = Splitter.SplitArgs("arg1 'quote \\' within quote' arg3");

            // then
            Assert.IsNotNull(args);
            Assert.AreEqual(3, args.Length);
            Assert.AreEqual("arg1", args[0]);
            Assert.AreEqual("quote ' within quote", args[1]);
            Assert.AreEqual("arg3", args[2]);

            // when
            args = Splitter.SplitArgs("arg1 \"dquote \\\" within dquote\" arg3");

            // then
            Assert.IsNotNull(args);
            Assert.AreEqual(3, args.Length);
            Assert.AreEqual("arg1", args[0]);
            Assert.AreEqual("dquote \" within dquote", args[1]);
            Assert.AreEqual("arg3", args[2]);

            // when
            args = Splitter.SplitArgs("arg1 \"quote ' within dquote\" arg3");

            // then
            Assert.IsNotNull(args);
            Assert.AreEqual(3, args.Length);
            Assert.AreEqual("arg1", args[0]);
            Assert.AreEqual("quote ' within dquote", args[1]);
            Assert.AreEqual("arg3", args[2]);
        }

        [Test]
        public void TestSpaces()
        {
            // given
            string[] args;

            // when
            args = Splitter.SplitArgs("        leading space");

            // then
            Assert.IsNotNull(args);
            Assert.AreEqual(2, args.Length);
            Assert.AreEqual("leading", args[0]);
            Assert.AreEqual("space", args[1]);

            // when
            args = Splitter.SplitArgs("trailing space        ");

            // then
            Assert.IsNotNull(args);
            Assert.AreEqual(2, args.Length);
            Assert.AreEqual("trailing", args[0]);
            Assert.AreEqual("space", args[1]);

            // when
            args = Splitter.SplitArgs("intermediate         space");

            // then
            Assert.IsNotNull(args);
            Assert.AreEqual(2, args.Length);
            Assert.AreEqual("intermediate", args[0]);
            Assert.AreEqual("space", args[1]);

            // when
            args = Splitter.SplitArgs("'  quoted         space  '");

            // then
            Assert.IsNotNull(args);
            Assert.AreEqual(1, args.Length);
            Assert.AreEqual("  quoted         space  ", args[0]);

            // when
            args = Splitter.SplitArgs("\"  dquoted         space  \"");

            // then
            Assert.IsNotNull(args);
            Assert.AreEqual(1, args.Length);
            Assert.AreEqual("  dquoted         space  ", args[0]);
        }

        [Test]
        public void TestUnmatchedQuotes()
        {
            // given
            string[] args = null;

            // when
            var e = Assert.Throws<Splitter.UnmatchedQuoteException>(() => args = Splitter.SplitArgs("arg1 'unmatched quote string"));

            // then
            Assert.IsNotNull(e);
            Assert.AreEqual('\'', e.Delimiter);
            Assert.AreEqual(5, e.Index);
            Assert.IsNull(args);

            // when
            e = Assert.Throws<Splitter.UnmatchedQuoteException>(() => args = Splitter.SplitArgs("arg1 \"unmatched dquote string"));

            // then
            Assert.IsNotNull(e);
            Assert.AreEqual('"', e.Delimiter);
            Assert.AreEqual(5, e.Index);
            Assert.IsNull(args);
        }

        [Test]
        public void TestMixedQuotedAndUnquoted()
        {
            // given
            string[] args;

            // when
            args = Splitter.SplitArgs(" arg1 'mixed'quoted\"and \\\" unquoted\" arg3");

            // then
            Assert.IsNotNull(args);
            Assert.AreEqual(3, args.Length);
            Assert.AreEqual("arg1", args[0]);
            Assert.AreEqual("mixedquotedand \" unquoted", args[1]);
            Assert.AreEqual("arg3", args[2]);
        }
    }
}

