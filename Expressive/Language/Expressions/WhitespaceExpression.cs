using System.Collections.Generic;
using System.Linq;
using Expressive.Core.Language.Interpreter;

namespace Expressive.Core.Language.Expressions
{
    public class WhitespaceExpression : TerminatingExpression
    {
        public override Production Parse(List<Token> tokens)
        {
            var token = tokens.First();
            if (token.TokenClass != TokenClass.Whitespace)
                return new Production(null, tokens);
            Token = token;
            return new Production(this, tokens.ToList().Skip(1).ToList());
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values,
            FunctionSource functions)
            => ToString();
    }
}
