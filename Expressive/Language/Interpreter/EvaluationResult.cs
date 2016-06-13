using System;
using System.Collections.Generic;
using Expressive.Core.Language.Types;

namespace Expressive.Core.Language.Interpreter
{
    /// <summary>
    /// Encapsulates the result of evaluating an Expressive expression, or the value/result of an external function
    /// or replacement symbol.
    /// Implictly convertable from all supported .NET types.
    /// All List types contained within will be of type EvaluationResult.
    /// <see cref="EvaluationType"/>
    /// </summary>
    public class EvaluationResult
    {
        public EvaluationType Type { get; protected set; }
        public object Result { get; protected set; }

        public EvaluationResult(EvaluationType type, object result)
        {
            Type = type;
            Result = result;
        }

        public string AsString()
        {
            if (Type == EvaluationType.String) return (string) Result;
            return Result == null ? "" : Result.ToString();
        }

        public int? AsInt()
        {
            if (Type == EvaluationType.Int) return (int)Result;
            return Result as int?;
        }

        public float? AsFloat()
        {
            if (Result is float || Result is decimal) Result = new RealNumber(NumericPrecision.Float, Result);
            var floatNumber = Result as RealNumber;
            if (floatNumber == null) return null;
            return floatNumber.AsFloat();
        }

        public decimal? AsDecimal()
        {
            if (Result is float || Result is decimal) Result = new RealNumber(NumericPrecision.Decimal, Result);
            var floatNumber = Result as RealNumber;
            if (floatNumber == null) return null;
            return floatNumber.AsDecimal();
        }

        public RealNumber AsRealNumber(NumericPrecision precision)
        {
            if (Result is float || Result is decimal) return new RealNumber(precision, Result);
            if (Result is int) return RealNumber.FromInt(AsInt().Value, precision);
            return Result as RealNumber;
        }

        public DateTime? AsDateTime()
        {
            return Result as DateTime?;
        }

        public bool? AsBoolean()
        {
            return Result as bool?;
        }

        public List<EvaluationResult> AsList() => Result as List<EvaluationResult>;

        public bool IsNull()
        {
            return Type == EvaluationType.Null || Result == null;
        }
        
        public static implicit operator EvaluationResult(bool value) => new EvaluationResult(EvaluationType.Boolean, value);
        public static implicit operator EvaluationResult(int value) => new EvaluationResult(EvaluationType.Int, value);
        public static implicit operator EvaluationResult(double value) => new EvaluationResult(EvaluationType.Float, value);
        public static implicit operator EvaluationResult(float value) => new EvaluationResult(EvaluationType.Float, value);
        public static implicit operator EvaluationResult(decimal value) => new EvaluationResult(EvaluationType.Float, value);
        public static implicit operator EvaluationResult(DateTime value) => new EvaluationResult(EvaluationType.DateTime, value);
        public static implicit operator EvaluationResult(string value) => new EvaluationResult(EvaluationType.String, value);
        public static implicit operator EvaluationResult(List<EvaluationResult> value) => new EvaluationResult(EvaluationType.Enumerable, value ?? new List<EvaluationResult>());
    }
}
