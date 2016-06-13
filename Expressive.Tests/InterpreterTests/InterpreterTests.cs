using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Core.Language.Interpreter;
using Expressive.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Expressive.Tests.InterpreterTests
{
    [TestClass]
    public class InterpreterTests : ExpressionInterpreter
    {
        [TestMethod]
        public void InterpreterFailsGracefully()
        {
            Assert.IsTrue(FailsGracefully(() => EvaluateOperation(@"100 +", NumericPrecision.Decimal)));
            Assert.IsTrue(FailsGracefully(() => EvaluateOperation(@"####123\\21!!!axsdokl1234", NumericPrecision.Decimal)));
            Assert.IsTrue(FailsGracefully(() => EvaluateOperation(@"15 + - 4", NumericPrecision.Decimal)));
            Assert.IsTrue(FailsGracefully(() => EvaluateOperation(@"(1, 2, 3", NumericPrecision.Decimal)));
        }

        [TestMethod]
        public void CanInterpretChainedExpressions()
        {
            var value = EvaluateOperation(@"100 + 3 / 3", NumericPrecision.Decimal);
            Assert.AreEqual(EvaluationType.Int, value.Type);
            Assert.AreEqual(101, value.AsInt().Value);
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"(100 + 3) / 3", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(34, value.AsInt().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"(99 + 1) * (2 * 2)", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(400, value.AsInt().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"(3 / 3) - -5", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(6, value.AsInt().Value);
            }
        }

        [TestMethod]
        public void CanPerformBasicIntegerOperations()
        {
            //miss out time trial for the first eval, as this includes the start-up time for the interpreter etc
            var value = EvaluateOperation(@"100 + 3", NumericPrecision.Decimal);
            Assert.AreEqual(EvaluationType.Int, value.Type);
            Assert.AreEqual(103, value.AsInt().Value);
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100 - 3", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(97, value.AsInt().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100 * 3", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(300, value.AsInt().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100 / 3", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(33, value.AsInt().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100 != 3", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(true, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100 = 3", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(false, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100 < 3", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(false, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100 > 3", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(true, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100 <= 3", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(false, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100 >= 3", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(true, value.AsBoolean().Value);
            }
        }

        [TestMethod]
        public void CanPerformMixedMathematicalOperations()
        {
            //miss out time trial for the first eval, as this includes the start-up time for the interpreter etc
            var value = EvaluateOperation(@"100.00 + 3", NumericPrecision.Float);
            Assert.AreEqual(EvaluationType.Float, value.Type);
            Assert.AreEqual(103, value.AsFloat().Value);
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100 - 3.00", NumericPrecision.Float);
                Assert.AreEqual(EvaluationType.Float, value.Type);
                Assert.AreEqual(97, value.AsFloat().Value);
            }
        }

        [TestMethod]
        public void CanPerformBasicFloatOperations()
        {
            //miss out time trial for the first eval, as this includes the start-up time for the interpreter etc
            var value = EvaluateOperation(@"100.00 + 3.00", NumericPrecision.Float);
            Assert.AreEqual(EvaluationType.Float, value.Type);
            Assert.AreEqual(103, value.AsFloat().Value);
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 - 3.00", NumericPrecision.Float);
                Assert.AreEqual(EvaluationType.Float, value.Type);
                Assert.AreEqual(97, value.AsFloat().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 * 3.00", NumericPrecision.Float);
                Assert.AreEqual(EvaluationType.Float, value.Type);
                Assert.AreEqual(300, value.AsFloat().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 / 3.00", NumericPrecision.Float);
                Assert.AreEqual(EvaluationType.Float, value.Type);
                Assert.AreEqual(33.33, Math.Round(value.AsFloat().Value), 2);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 != 3.00", NumericPrecision.Float);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(true, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 = 3.00", NumericPrecision.Float);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(false, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 < 3.00", NumericPrecision.Float);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(false, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 > 3.00", NumericPrecision.Float);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(true, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 <= 3.00", NumericPrecision.Float);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(false, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 >= 3.00", NumericPrecision.Float);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(true, value.AsBoolean().Value);
            }
        }

        [TestMethod]
        public void CanUseBasicVariables()
        {
            //miss out time trial for the first eval, as this includes the start-up time for the interpreter etc
            var value = EvaluateOperation(@"100.00 + [a]", NumericPrecision.Decimal, new Dictionary<string, object> { { "[a]", 3 } }, null);
            Assert.AreEqual(EvaluationType.Float, value.Type);
            Assert.AreEqual(103m, value.AsDecimal().Value);
            using (var timer = new TimeAssertion(milliseconds: 2))
            {
                value = EvaluateOperation(@"100.00 - [three]", NumericPrecision.Decimal, new Dictionary<string, object> { { "[three]", 3 } }, null);
                Assert.AreEqual(EvaluationType.Float, value.Type);
                Assert.AreEqual(97m, value.AsDecimal().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 2))
            {
                value = EvaluateOperation(@"[ten] * [three]", NumericPrecision.Decimal, new Dictionary<string, object>
                {
                    { "[three]", 3 },
                    { "[ten]", 10 }
                }, null);
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(30, value.AsInt().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 2))
            {
                value = EvaluateOperation(@"([ten] * [three]) / [five]", NumericPrecision.Decimal, new Dictionary<string, object>
                {
                    { "[three]", 3 },
                    { "[ten]", 10 },
                    { "[five]", 5 }
                }, null);
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(6, value.AsInt().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 2))
            {
                value = EvaluateOperation(@"[title] + [name]", NumericPrecision.Decimal, new Dictionary<string, object>
                {
                    { "[title]", "Captain " },
                    { "[name]", "Foo" }
                }, null);
                Assert.AreEqual(EvaluationType.String, value.Type);
                Assert.AreEqual("Captain Foo", value.AsString());
            }
            using (var timer = new TimeAssertion(milliseconds: 5))
            {
                value = EvaluateOperation(@"[formula] + 10", NumericPrecision.Decimal, new ValueSource
                {
                    { "[formula]", new EvaluationResult(EvaluationType.Expression, "(3 + 1) / 2") }
                }, null);
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(12, value.AsInt());
            }
        }

        [TestMethod]
        public void CanUseBasicFunctions()
        {
            EvaluationResult value = null;
            //miss out time trial for the first eval, as this includes the start-up time for the interpreter etc
            value = EvaluateOperation(@"SumInts([three], 2, 1)", NumericPrecision.Decimal,
                new Dictionary<string, object> { { "[three]", 3 } },
                new Dictionary<string, ExternalFunction> { { "SumInts", SumInts } });
            Assert.AreEqual(EvaluationType.Int, value.Type);
            Assert.AreEqual(6, value.AsInt().Value);
            using (var timer = new TimeAssertion(milliseconds: 5))
            {
                value = EvaluateOperation(@"SumInts([three], 2) + 1", NumericPrecision.Decimal,
                    new Dictionary<string, object> { { "[three]", 3 } },
                    new Dictionary<string, ExternalFunction> { { "SumInts", SumInts } });
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(6, value.AsInt().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 5))
            {
                value = EvaluateOperation(@"SumInts([three], ((2 + 4) * 2) / 2) + 1", NumericPrecision.Decimal,
                    new Dictionary<string, object> { { "[three]", 3 } },
                    new Dictionary<string, ExternalFunction> { { "SumInts", SumInts } });
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(10, value.AsInt().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 5))
            {
                value = EvaluateOperation(@"ListFactory()", NumericPrecision.Decimal,
                    null,
                    new Dictionary<string, ExternalFunction> { { "ListFactory", ListFactory } });
                Assert.AreEqual(EvaluationType.Enumerable, value.Type);
                Assert.IsTrue(value.AsList().Select(i => i.Result).SequenceEqual(new List<object> { 1, 2, 3 }));
            }
            using (var timer = new TimeAssertion(milliseconds: 5))
            {
                value = EvaluateOperation(@"IncrementInt(1)", NumericPrecision.Decimal,
                    null,
                    new Dictionary<string, ExternalFunction> { { "IncrementInt", IncrementInt } });
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(2, value.AsInt().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 10))
            {
                value = EvaluateOperation(@"ExpressionFactoryResultingInTwo()", NumericPrecision.Decimal,
                    new Dictionary<string, object> { { "[two]", 2 } }, 
                    new Dictionary<string, ExternalFunction> { { "ExpressionFactoryResultingInTwo", ExpressionFactoryResultingInTwo } });
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(2, value.AsInt().Value);
            }
        }

        [TestMethod]
        public void CanPerformBasicDecimalOperations()
        {
            //miss out time trial for the first eval, as this includes the start-up time for the interpreter etc
            var value = EvaluateOperation(@"100.00 + 3.00", NumericPrecision.Decimal);
            Assert.AreEqual(EvaluationType.Float, value.Type);
            Assert.AreEqual(103m, value.AsDecimal().Value);
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 - 3.00", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Float, value.Type);
                Assert.AreEqual(97m, value.AsDecimal().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 * 3.00", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Float, value.Type);
                Assert.AreEqual(300m, value.AsDecimal().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 / 3.00", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Float, value.Type);
                Assert.AreEqual(33.33m, Math.Round(value.AsDecimal().Value, 2));
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 != 3.00", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(true, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 = 3.00", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(false, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 < 3.00", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(false, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 > 3.00", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(true, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 <= 3.00", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(false, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"100.00 >= 3.00", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(true, value.AsBoolean().Value);
            }
        }

        [TestMethod]
        public void CanPerformBasicStringOperations()
        {
            //miss out time trial for the first eval, as this includes the start-up time for the interpreter etc
            var value = EvaluateOperation(@"'hi' + ' there'", NumericPrecision.Decimal);
            Assert.AreEqual(EvaluationType.String, value.Type);
            Assert.AreEqual("hi there", value.AsString());
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"'yo bro' - ' bro'", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.String, value.Type);
                Assert.AreEqual("yo", value.AsString());
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"'test' != 'testy'", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(true, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"'test' = 'testy'", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(false, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"'longer string' < 'short'", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(false, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"'longer string' > 'short'", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(true, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"'longer string' <= 'short'", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(false, value.AsBoolean().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 1))
            {
                value = EvaluateOperation(@"'longer string' >= 'short'", NumericPrecision.Decimal);
                Assert.AreEqual(EvaluationType.Boolean, value.Type);
                Assert.AreEqual(true, value.AsBoolean().Value);
            }
        }
    }
}
