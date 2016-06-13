using System.Collections.Generic;
using System.Linq;

namespace Expressive.Core.Language.Expressions
{
    public abstract class NumericExpression : TerminatingExpression
    {
        public TokenClass NumericType { get; set; }

        protected NumericExpression(TokenClass numericType)
        {
            NumericType = numericType;
        }

        public override Production Parse(List<Token> tokens)
        {
            var negative = Negative(tokens);
            if (negative.Expression != null) return negative;
            var first = tokens.First();
            if (first.TokenClass != NumericType) return new Production(null, tokens);
            Token = first;
            return new Production(this, tokens.Skip(1).ToList());
        }

        public virtual Production Negative(List<Token> tokens)
        {
            if (tokens.Count < 2) return new Production(null, tokens);
            var op = tokens.First();
            if (op.TokenClass != TokenClass.Operator || op.Lexeme != "-") return new Production(null, tokens);
            var number = tokens.ElementAt(1);
            if (number.TokenClass != NumericType) return new Production(null, tokens);
            number.Lexeme = $"-{number.Lexeme}";
            Token = number;
            return new Production(this, tokens.Skip(2).ToList());
        }
    }
}
