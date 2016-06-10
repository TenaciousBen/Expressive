using System.Collections.Generic;
using System.Linq;
using Apirion.Expressive.Core.Language.Interpreter;

namespace Apirion.Expressive.Core.Language.Expressions
{
    public class OperatorExpression : TerminatingExpression
    {
        public override string ToString()
        {
            return base.Token.Lexeme.ToString();
        }

        public override Production Parse(List<Token> tokens)
        {
            if (tokens.First().TokenClass != TokenClass.Operator) return new Production(null, tokens);
            base.Token = tokens.First();
            var remainder = tokens.ToList().Skip(1).ToList();
            return new Production(this, remainder);
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values, FunctionSource functions)
            => ToString();
    }
}
