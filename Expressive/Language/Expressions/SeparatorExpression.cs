using System.Collections.Generic;
using System.Linq;
using Expressive.Core.Language.Interpreter;

namespace Expressive.Core.Language.Expressions
{
    public class SeparatorExpression : TerminatingExpression
    {
        public override Production Parse(List<Token> tokens)
        {
            var separator = tokens.First();
            if (separator.TokenClass != TokenClass.Separator) return new Production(null, tokens);
            var remainder = tokens.ToList().Skip(1).ToList();
            Token = separator;
            return new Production(this, remainder);
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values, FunctionSource functions)
            => ToString();
    }
}
