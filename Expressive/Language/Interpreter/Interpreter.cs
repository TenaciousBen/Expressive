using System;
using System.Collections.Generic;

namespace Expressive.Core.Language.Interpreter
{
    public class Interpreter
    {
        public NumericPrecision NumericPrecision { get; set; }
        public Dictionary<string, object> Values { get; set; }
        public Dictionary<string, ExternalFunction> Functions { get; set; }

        public Interpreter(NumericPrecision numericPrecision, Dictionary<string, object> values, Dictionary<string, ExternalFunction> functions)
        {
            NumericPrecision = numericPrecision;
            Values = values;
            Functions = functions;
        }

        public Interpreter()
            : this(NumericPrecision.Float, new Dictionary<string, object>(), new Dictionary<string, ExternalFunction>())
        {
        }

        public EvaluationResult Evaluate(string expression) => Evaluate(expression, NumericPrecision, Values, Functions);

        public static EvaluationResult Evaluate(string expression, NumericPrecision precision, ValueSource values, FunctionSource functions)
        {
            var tokens = Lexer.Lex(expression);
            var parsed = Parser.Parse(tokens);
            return parsed.Evaluate(precision, values, functions);
        }

        public static EvaluationResult Evaluate(string expression, NumericPrecision precision, Dictionary<string, object> values,
            Dictionary<string, ExternalFunction> functions)
        {
            var tokens = Lexer.Lex(expression);
            var parsed = Parser.Parse(tokens);
            var valueSource = ValueSourceFromDictionary(values);
            var functionSource = FunctionSourceFromDictionary(functions);
            return parsed.Evaluate(precision, valueSource, functionSource);
        }

        public static EvaluationResult Evaluate(string expression, NumericPrecision precision)
        {
            var tokens = Lexer.Lex(expression);
            var parsed = Parser.Parse(tokens);
            return parsed.Evaluate(precision, new ValueSource(), new FunctionSource());
        }

        private static ValueSource ValueSourceFromDictionary(Dictionary<string, object> values)
        {
            values = values ?? new Dictionary<string, object>();
            var valueSource = new ValueSource();
            foreach (var kvp in values)
            {
                var type = EvaluationType.Null;
                if (kvp.Value is float || kvp.Value is decimal) type = EvaluationType.Float;
                if (kvp.Value is int) type = EvaluationType.Int;
                if (kvp.Value is bool) type = EvaluationType.Boolean;
                if (kvp.Value is DateTime) type = EvaluationType.DateTime;
                if (kvp.Value is string) type = EvaluationType.String;
                if (type == EvaluationType.Null) throw new Exception($"Provided value '{kvp.Key}' was null");
                valueSource.TryAddValue(kvp.Key, new EvaluationResult(type, kvp.Value));
            }
            return valueSource;
        }

        private static FunctionSource FunctionSourceFromDictionary(Dictionary<string, ExternalFunction> functions)
        {
            functions = functions ?? new Dictionary<string, ExternalFunction>();
            var functionSource = new FunctionSource();
            foreach (var kvp in functions) functionSource.TryAddValue(kvp.Key, new Function(kvp.Value));
            return functionSource;
        }
    }
}
