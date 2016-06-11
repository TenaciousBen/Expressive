﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Apirion.Expressive.Core.Language;
using Apirion.Expressive.Core.Language.Expressions;
using Apirion.Expressive.Core.Language.Interpreter;
using Apirion.Expressive.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Apirion.Expressive.Tests.InterpreterTests
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
        }

        [TestMethod]
        public void CanUseBasicFunctions()
        {
            EvaluationResult value = null;
            using (var timer = new TimeAssertion(milliseconds: 5))
            {
                value = EvaluateOperation(@"SumInts([three], 2, 1)", NumericPrecision.Decimal, 
                    new Dictionary<string, object> { { "[three]", 3 } },
                    new Dictionary<string, ExternalFunction> { { "SumInts", SumInts } });
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(6, value.AsInt().Value);
            }
            using (var timer = new TimeAssertion(milliseconds: 5))
            {
                value = EvaluateOperation(@"SumInts([three], 2) + 1", NumericPrecision.Decimal,
                    new Dictionary<string, object> { { "[three]", 3 } },
                    new Dictionary<string, ExternalFunction> { { "SumInts", SumInts } });
                Assert.AreEqual(EvaluationType.Int, value.Type);
                Assert.AreEqual(6, value.AsInt().Value);
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