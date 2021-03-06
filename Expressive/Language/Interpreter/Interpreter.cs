﻿using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Core.Exceptions;
using Expressive.Core.Language.Expressions;

namespace Expressive.Core.Language.Interpreter
{
    /// <summary>
    /// The interpreter for the Expressive language.
    /// </summary>
    public class Interpreter
    {
        public NumericPrecision NumericPrecision { get; set; }
        public ValueSource Values { get; set; }
        public FunctionSource Functions { get; set; }

        public Interpreter(NumericPrecision numericPrecision, ValueSource values, FunctionSource functions)
        {
            NumericPrecision = numericPrecision;
            Values = values;
            Functions = functions;
        }

        public Interpreter(NumericPrecision numericPrecision, Dictionary<string, object> values,
            Dictionary<string, ExternalFunction> functions)
            : this(numericPrecision, ValueSourceFromDictionary(values), FunctionSourceFromDictionary(functions))
        {

        }

        public Interpreter(NumericPrecision numericPrecision)
            : this(numericPrecision, new ValueSource(), new FunctionSource())
        {
        }

        /// <summary>
        /// Defaults to Float precision.
        /// </summary>
        public Interpreter()
            : this(NumericPrecision.Float, new ValueSource(), new FunctionSource())
        {
        }

        /// <summary>
        /// Evalutes the parameter Expressive expression, passing in the NumericPrecision, Values and Functions properties as the context.
        /// </summary>
        public EvaluationResult Evaluate(string expression) => Evaluate(expression, NumericPrecision, Values, Functions);

        /// <summary>
        /// Evaluates the parameter Expressive expression, with the parameter NumericPrecision, ValueSource and FunctionSource.
        /// </summary>
        public static EvaluationResult Evaluate(string expression, NumericPrecision precision, ValueSource values, FunctionSource functions)
        {
            values = values ?? new ValueSource();
            functions = functions ?? new FunctionSource();
            var tokens = Lexer.Lex(expression);
            var parsed = Parser.Parse(tokens);
            if (parsed is ErrorExpression) throw new ParserException(((ErrorExpression)parsed).ErroneousTokens);
            return parsed.Evaluate(precision, values, functions);
        }

        /// <summary>
        /// Evaluates the parameter Expressive expression, with the parameter NumericPrecision, converting the parameter values 
        /// and functions dictionaries in ValueSource and FunctionSource objects.
        /// </summary>
        public static EvaluationResult Evaluate(string expression, NumericPrecision precision, Dictionary<string, object> values,
            Dictionary<string, ExternalFunction> functions)
        {
            var valueSource = ValueSourceFromDictionary(values);
            var functionSource = FunctionSourceFromDictionary(functions);
            return Evaluate(expression, precision, valueSource, functionSource);
        }

        /// <summary>
        /// Evaluates the parameter Expressive expression, with the parameter NumericPrecision and an empty ValueSource and FunctionSource.
        /// </summary>
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
