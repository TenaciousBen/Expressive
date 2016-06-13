using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Expressive.Core.Language;
using Expressive.Core.Language.Expressions;
using Expressive.Core.Language.Interpreter;

namespace Expressive.Console
{
    public static class TypeResolver
    {
        private readonly static Regex AssignmentRegex = new Regex("^(.+)=(.+)$");

        public static bool IsAssignment(string expression) => AssignmentRegex.Match(expression).Success;

        public static RecognisedType Resolve(string expression, ValueSource values, FunctionSource functions)
        {
            var nameAndValue = expression.Split(new[] {'='}, 2);
            if (nameAndValue.Length != 2) return new RecognisedType();
            var name = nameAndValue[0]?.Trim();
            var value = nameAndValue[1]?.Trim();
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(value)) return new RecognisedType();
            int parsedInt;
            if (int.TryParse(value, out parsedInt)) return new RecognisedType(name, parsedInt);
            decimal parsedDecimal;
            if (decimal.TryParse(value, out parsedDecimal)) return new RecognisedType(name, parsedDecimal);
            float parsedFloat;
            if (float.TryParse(value, out parsedFloat)) return new RecognisedType(name, parsedFloat);
            DateTime parsedDateTime;
            if (DateTime.TryParse(value, out parsedDateTime)) return new RecognisedType(name, parsedDateTime);
            bool parsedBool;
            if (bool.TryParse(value, out parsedBool)) return new RecognisedType(name, parsedBool);
            var str = AsString(value);
            if (str != null) return new RecognisedType(name, str);
            var list = AsList(value, values, functions);
            if (list != null) return new RecognisedType(name, list);
            var expressionString = AsExpression(value);
            if (expressionString != null) return new RecognisedType(name, new EvaluationResult(EvaluationType.Expression, expressionString));
            return new RecognisedType();
        }

        private static string AsExpression(string value)
        {
            var tokens = Lexer.Lex(value);
            if (tokens.Any(t => t.TokenClass == TokenClass.Error)) return null;
            return value;
        }

        private static string AsString(string value)
        {
            var tokens = Lexer.Lex(value);
            if (tokens.Count > 1 || tokens.First().TokenClass != TokenClass.String) return null;
            return value;
        }

        //hacky approach to parse out a list from a string, will fail if the list contains undeclared variables
        private static List<EvaluationResult> AsList(string value, ValueSource values, FunctionSource functions)
        {
            var tokens = Lexer.Lex(value);
            //can't be a list if it doesn't start and end with scope
            if (tokens.First().TokenClass != TokenClass.StartScope || tokens.Last().TokenClass != TokenClass.EndScope) return null;
            var separated = new SeparatedExpression().Parse(tokens);
            //can't be a list if it wasn't recognised by the list parser
            if (separated.Expression == null) return null;
            return Interpreter.Evaluate(value, NumericPrecision.Float, values, functions).AsList();
        }
    }

    public class RecognisedType
    {
        public bool WasRecognised { get; set; }
        public string Name { get; set; }
        public EvaluationResult Resolved { get; set; }

        public RecognisedType(string name, EvaluationResult resolved)
        {
            Name = name;
            Resolved = resolved;
            WasRecognised = resolved != null;
        }

        public RecognisedType() 
            : this(null, null)
        {
            
        }
    }
}
