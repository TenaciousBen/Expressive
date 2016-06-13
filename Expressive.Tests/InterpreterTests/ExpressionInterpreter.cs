using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Core.Exceptions;
using Expressive.Core.Language.Interpreter;

namespace Expressive.Tests.InterpreterTests
{
    public class ExpressionInterpreter
    {
        public EvaluationResult EvaluateOperation(string expression, NumericPrecision precision, Dictionary<string, object> values, 
            Dictionary<string, ExternalFunction> functions)
        {
            return Interpreter.Evaluate(expression, precision, values, functions);
        }

        public EvaluationResult EvaluateOperation(string expression, NumericPrecision precision, ValueSource values,
            FunctionSource functions)
        {
            return Interpreter.Evaluate(expression, precision, values, functions);
        }


        public EvaluationResult EvaluateOperation(string expression, NumericPrecision precision)
        {
            return Interpreter.Evaluate(expression, precision);
        }

        public bool FailsGracefully(Action action)
        {
            Exception thrown = null;
            try
            {
                action();
            }
            catch (Exception e)
            {
                thrown = e;
            }
            if (thrown == null) throw new Exception("No exception was thrown");
            return thrown is LexerException || thrown is ParserException || thrown is EvaluationException;
        }

        public EvaluationResult SumInts(EvaluationResult result)
        {
            if (result.Type != EvaluationType.Enumerable) throw new Exception();
            int sum = 0;
            var enumerable = result.Result as IEnumerable<EvaluationResult>;
            if (enumerable == null || enumerable.Any(i => i.Type != EvaluationType.Int)) throw new Exception();
            foreach (var element in enumerable) sum += (int)element.Result;
            return sum;
        }
    }
}
