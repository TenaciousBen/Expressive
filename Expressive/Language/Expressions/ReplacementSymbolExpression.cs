using System.Collections.Generic;
using System.Linq;
using Expressive.Core.Exceptions;
using Expressive.Core.Language.Interpreter;

namespace Expressive.Core.Language.Expressions
{
    public class ReplacementSymbolExpression : TerminatingExpression
    {
        public override Production Parse(List<Token> tokens)
        {
            var first = tokens.First();
            if (first.TokenClass != TokenClass.ReplacementSymbol)
                return new Production(null, tokens);
            Token = first;
            return new Production(this, tokens.ToList().Skip(1).ToList());
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values,
            FunctionSource functions)
        {
            var resolved = values.TryGetValue(ToString());
            if (resolved == null) throw new UnmatchedSymbolException(ToString());
            if (resolved.Type != EvaluationType.Expression) return resolved;
            return Interpreter.Interpreter.Evaluate(resolved.AsString(), numericPrecision, values, functions);
        }
    }
}
