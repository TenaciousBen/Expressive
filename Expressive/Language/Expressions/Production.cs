using System.Collections.Generic;

namespace Expressive.Core.Language.Expressions
{
    public class Production
    {
        public Expression Expression { get; set; }
        public List<Token> RemainingTokens { get; set; }

        public Production(Expression expression, List<Token> remainingTokens)
        {
            Expression = expression;
            RemainingTokens = remainingTokens;
        }
    }
}