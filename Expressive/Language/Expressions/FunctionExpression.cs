using System.Collections.Generic;
using System.Linq;
using Apirion.Expressive.Core.Exceptions;
using Apirion.Expressive.Core.Extensions;
using Apirion.Expressive.Core.Language.Interpreter;

namespace Apirion.Expressive.Core.Language.Expressions
{
    public class FunctionExpression : Expression
    {
        public override Production Parse(List<Token> tokens)
        {
            var symbolToken = tokens.First();
            if (symbolToken.TokenClass != TokenClass.Symbol) return new Production(null, tokens);
            var symbol = new SymbolExpression().Parse(tokens);
            var remaining = symbol.RemainingTokens;
            //function has to have parameters
            if (symbol.Expression == null || remaining.None()) return new Production(null, tokens);
            Constituents.Add(symbol.Expression);
            var separated = new SeparatedExpression().Parse(symbol.RemainingTokens);
            if (separated.Expression != null)
            {
                Constituents.Add(separated.Expression);
                return new Production(this, separated.RemainingTokens);
            }
            var scoped = new ScopedExpression().Parse(symbol.RemainingTokens);
            if (scoped.Expression == null) return new Production(null, tokens);
            Constituents.Add(scoped.Expression);
            return new Production(this, scoped.RemainingTokens);
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values, FunctionSource functions)
        {
            var symbol = Constituents[0] as SymbolExpression;
            if (symbol == null) throw new EvaluationException(this);
            var matchingFunction = functions.TryGetValue(symbol.ToString());
            if (matchingFunction == null) throw new UnmatchedSymbolException(symbol.ToString());
            var parameters = Constituents[1].Evaluate(numericPrecision, values, functions);
            return matchingFunction.Invoke(parameters);
        }
    }
}