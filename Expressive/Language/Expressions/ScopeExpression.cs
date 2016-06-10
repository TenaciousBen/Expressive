using System.Collections.Generic;
using System.Linq;
using Apirion.Expressive.Core.Language.Interpreter;

namespace Apirion.Expressive.Core.Language.Expressions
{
    public class ScopeExpression : TerminatingExpression
    {
        public override Production Parse(List<Token> tokens)
        {
            var firstParen = tokens.First();
            if (firstParen.TokenClass != TokenClass.StartScope && firstParen.TokenClass != TokenClass.EndScope)
                return new Production(null, tokens);
            Token = firstParen;
            var remaining = tokens.ToList().Skip(1).ToList();
            return new Production(this, remaining);
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values,
            FunctionSource functions)
            => ToString();
    }
}
