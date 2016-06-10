using System.Collections.Generic;
using System.Linq;
using Apirion.Expressive.Core.Exceptions;
using Apirion.Expressive.Core.Extensions;
using Apirion.Expressive.Core.Language.Interpreter;

namespace Apirion.Expressive.Core.Language.Expressions
{
    public class ErrorExpression : TerminatingExpression
    {
        public List<Token> ErroneousTokens { get; set; }

        public ErrorExpression()
        {
            ErroneousTokens = new List<Token>();
        }

        public override string ToString() { return ErroneousTokens.Select(t => t.Lexeme).StringConcat(); }

        public override Production Parse(List<Token> tokens)
        {
            ErroneousTokens = tokens;
            return new Production(this, new List<Token>());
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values, FunctionSource functions)
        {
            throw new EvaluationException(this);
        }

        public Production Parse(Expression successfullyParsed, List<Token> tokens)
        {
            Constituents.Add(successfullyParsed);
            ErroneousTokens = tokens;
            return new Production(this, new List<Token>());
        }
    }
}
