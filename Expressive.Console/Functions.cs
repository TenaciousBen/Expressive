using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expressive.Core.Language.Interpreter;

namespace Expressive.Console
{
    public class Functions
    {
        public static EvaluationResult Sum(EvaluationResult numbers)
        {
            if (numbers == null) throw new ArgumentNullException(nameof(numbers));
            if (numbers.Type != EvaluationType.Enumerable) throw new Exception("Sum expects a list of numerics");
            var list = numbers.AsList();
            if (list.Any(n => n.Type != EvaluationType.Int && n.Type != EvaluationType.Float)) throw new Exception("Sum expects a list of numerics");
            float sum = 0;
            foreach (var n in list) sum += n.AsFloat().Value;
            return sum;
        }

        public static EvaluationResult Increment(EvaluationResult number)
        {
            if (number == null) throw new ArgumentNullException(nameof(number));
            if (number.Type == EvaluationType.Int) return number.AsInt().Value + 1;
            if (number.Type == EvaluationType.Float) return number.AsFloat().Value + 1f;
            throw new Exception("Increment expects a number");
        }

        public static EvaluationResult Decrement(EvaluationResult number)
        {
            if (number == null) throw new ArgumentNullException(nameof(number));
            if (number.Type == EvaluationType.Int) return number.AsInt().Value - 1;
            if (number.Type == EvaluationType.Float) return number.AsFloat().Value - 1f;
            throw new Exception("Decrement expects a number");
        }

        public static EvaluationResult Upper(EvaluationResult str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (str.Type != EvaluationType.String) throw new Exception("Upper expects a string");
            return str.AsString()?.ToUpper();
        }

        public static EvaluationResult Lower(EvaluationResult str)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));
            if (str.Type != EvaluationType.String) throw new Exception("Lower expects a string");
            return str.AsString()?.ToLower();
        }

        public static FunctionSource FunctionSource()
        {
            return new FunctionSource
            {
                { "Sum", new Function(Sum) },
                { "Increment", new Function(Increment) },
                { "Decrement", new Function(Decrement) },
                { "Upper", new Function(Upper) },
                { "Lower", new Function(Lower) },
            };
        }
    }
}
