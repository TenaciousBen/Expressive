using System.Collections.Generic;
using System.Linq;
using Apirion.Expressive.Core.Language.Interpreter;

namespace Apirion.Expressive.Core.Language.Expressions
{
    public class NullExpression : TerminatingExpression
    {
        public override string ToString()
        {
            return "";
        }

        public override Production Parse(List<Token> tokens)
        {
            if (tokens != null && tokens.Any()) return new Production(null, tokens);
            return new Production(this, tokens);
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values, FunctionSource functions)
            => new EvaluationResult(EvaluationType.Null, null);
    }
}
