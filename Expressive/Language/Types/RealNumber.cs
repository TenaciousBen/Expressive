using System;
using Expressive.Core.Language.Interpreter;

namespace Expressive.Core.Language.Types
{
    /// <summary>
    /// Encapsulates real numbers, so that the interpreter can evaluate real numbers
    /// as either a float or decimal depending on the supplied numerical precision.
    /// <see cref="Interpreter.NumericPrecision"/>
    /// </summary>
    public class RealNumber : MathematicalValue<RealNumber>
    {
        public NumericPrecision NumericPrecision { get; set; }

        public RealNumber(NumericPrecision numericPrecision, object value)
        {
            if (!(value is float) && !(value is decimal)) throw new ArgumentException("Value must be float or decimal");
            NumericPrecision = numericPrecision;
            Value = value;
        }

        public override int CompareTo(RealNumber other)
        {
            if (other == null)
                return 1; //any value is greater than no value
            if (this.NumericPrecision == NumericPrecision.Decimal)
                return this.AsDecimal() > other.AsDecimal() ? 1 : this.AsDecimal() < other.AsDecimal() ? -1 : 0;
            if (this.NumericPrecision == NumericPrecision.Float)
                return this.AsFloat() > other.AsFloat() ? 1 : this.AsFloat() < other.AsFloat() ? -1 : 0;
            return 0;
        }

        public decimal? AsDecimal()
        {
            if (NumericPrecision == NumericPrecision.Decimal) return (decimal)Value;
            if (NumericPrecision == NumericPrecision.Float && (float)Value < (float)Decimal.MaxValue &&
                (float)Value > (float)Decimal.MinValue) return (decimal)Value; // a float may fit inside a decimal
            return null; // a float may also be outside of the valid range of a decimal
        }

        public float AsFloat()
        {
            if (NumericPrecision == NumericPrecision.Float) return (float)Value;
            return (float)Value; // a decimal can fit inside a float with a loss of precision
        }

        public override RealNumber Add(RealNumber value)
        {
            if ((object) value == null) return this;
            if (NumericPrecision == NumericPrecision.Decimal) return new RealNumber(NumericPrecision.Decimal, this.AsDecimal() + value.AsDecimal());
            return new RealNumber(NumericPrecision.Float, this.AsFloat() + value.AsFloat());
        }

        public override RealNumber Subtract(RealNumber value)
        {
            if ((object)value == null) return this;
            if (NumericPrecision == NumericPrecision.Decimal) return new RealNumber(NumericPrecision.Decimal, this.AsDecimal() - value.AsDecimal());
            return new RealNumber(NumericPrecision.Float, this.AsFloat() - value.AsFloat());
        }

        public override RealNumber Multiply(RealNumber value)
        {
            if ((object)value == null) return this;
            if (NumericPrecision == NumericPrecision.Decimal) return new RealNumber(NumericPrecision.Decimal, this.AsDecimal() * value.AsDecimal());
            return new RealNumber(NumericPrecision.Float, this.AsFloat() * value.AsFloat());
        }

        public override RealNumber Divide(RealNumber value)
        {
            if ((object)value == null) return this;
            if (NumericPrecision == NumericPrecision.Decimal) return new RealNumber(NumericPrecision.Decimal, this.AsDecimal() / value.AsDecimal());
            return new RealNumber(NumericPrecision.Float, this.AsFloat() / value.AsFloat());
        }

        public static RealNumber FromInt(int value, NumericPrecision precision) 
            => precision == NumericPrecision.Decimal ? new RealNumber(precision, (decimal)value) : new RealNumber(precision, (float)value);
    }
}