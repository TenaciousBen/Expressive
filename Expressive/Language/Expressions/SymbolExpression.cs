using System.Collections.Generic;
using System.Linq;
using Apirion.Expressive.Core.Language.Interpreter;

namespace Apirion.Expressive.Core.Language.Expressions
{
    public class SymbolExpression : TerminatingExpression
    {
        public override Production Parse(List<Token> tokens)
        {
            var first = tokens.First();
            if (first.TokenClass != TokenClass.Symbol)
                return new Production(null, tokens);
            Token = first;
            return new Production(this, tokens.ToList().Skip(1).ToList());
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values,
            FunctionSource functions)
            => ToString();
    }
}
