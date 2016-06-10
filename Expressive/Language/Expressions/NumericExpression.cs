using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apirion.Expressive.Core.Extensions;

namespace Apirion.Expressive.Core.Language.Expressions
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
            var first = tokens.First();
            if (first.TokenClass != NumericType) return new Production(null, tokens);
            Token = first;
            return new Production(this, tokens.Skip(1).ToList());
        }
    }
}
