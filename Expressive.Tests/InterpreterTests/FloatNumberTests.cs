using Expressive.Core.Language.Interpreter;
using Expressive.Core.Language.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Expressive.Tests.InterpreterTests
{
    [TestClass]
    public class FloatNumberTests
    {
        [TestMethod]
        public void CanAddFloatNumbers()
        {
            var a = new RealNumber(NumericPrecision.Decimal, 500m);
            var b = new RealNumber(NumericPrecision.Decimal, 750.5m);
            Assert.AreEqual(500m + 750.5m, a.Add(b).AsDecimal().Value);
            Assert.AreEqual(500m + 750.5m, (a + b).AsDecimal().Value);
            a = new RealNumber(NumericPrecision.Float, 123.45f);
            b = new RealNumber(NumericPrecision.Float, 543.21f);
            Assert.AreEqual(123.45f + 543.21f, a.Add(b).AsFloat());
            Assert.AreEqual(123.45f + 543.21f, (a + b).AsFloat());
        }
    }
}
