using System.Collections.Generic;
using System.Linq;
using System.Text;
using Expressive.Core.Language.Interpreter;

namespace Expressive.Core.Language.Expressions
{
    public class SeparatedExpression : ScopedExpression
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("(");
            for (int i = 0; i < Constituents.Count; i++)
            {
                sb.Append(Constituents[i].ToString());
                if (i < (Constituents.Count - 1))
                    sb.Append(", ");
            }
            sb.Append(")");
            return sb.ToString();
        }

        public override Production Parse(List<Token> tokens)
        {
            var firstParen = tokens.First();
            if (firstParen.TokenClass != TokenClass.StartScope)
                return new Production(null, tokens);
            var current = new ScopeExpression().Parse(tokens);
            var remaining = current.RemainingTokens.ToList();
            while (remaining.Any() && remaining.FirstOrDefault() != null && remaining[0].TokenClass != TokenClass.EndScope)
            {
                Expression parsed;
                switch (remaining.First().TokenClass)
                {
                    case TokenClass.ReplacementSymbol:
                        if (Constituents.Count > 1 && !(Constituents.Last() is SeparatorExpression)) return new Production(null, tokens);
                        current = new ReplacementSymbolExpression().Parse(remaining);
                        break;
                    case TokenClass.Integer:
                        if (Constituents.Count > 1 && !(Constituents.Last() is SeparatorExpression)) return new Production(null, tokens);
                        current = new IntegerExpression().Parse(remaining);
                        break;
                    case TokenClass.Float:
                        if (Constituents.Count > 1 && !(Constituents.Last() is SeparatorExpression)) return new Production(null, tokens);
                        current = new FloatExpression().Parse(remaining);
                        break;
                    case TokenClass.String:
                        if (Constituents.Count > 1 && !(Constituents.Last() is SeparatorExpression)) return new Production(null, tokens);
                        current = new StringExpression().Parse(remaining);
                        break;
                    case TokenClass.Boolean:
                        if (Constituents.Count > 1 && !(Constituents.Last() is SeparatorExpression)) return new Production(null, tokens);
                        current = new BooleanExpression().Parse(remaining);
                        break;
                    case TokenClass.Symbol:
                        if (Constituents.Count > 1 && !(Constituents.Last() is SeparatorExpression)) return new Production(null, tokens);
                        current = new FunctionExpression().Parse(remaining);;
                        if (current.Expression != null) break;
                        current = new SymbolExpression().Parse(remaining);
                        break;
                    case TokenClass.StartScope:
                        if (Constituents.Count > 1 && !(Constituents.Last() is SeparatorExpression)) return new Production(null, tokens);
                        current = new SeparatedExpression().Parse(remaining);
                        if (current.Expression != null) break;
                        current = new ScopedExpression().Parse(remaining);
                        if (current.Expression != null) break;
                        break;
                    case TokenClass.EndScope:
                        current = new ScopeExpression().Parse(remaining);
                        break;
                    case TokenClass.Whitespace:
                        current = new WhitespaceExpression().Parse(remaining);
                        break;
                    case TokenClass.Separator:
                        current = new SeparatorExpression().Parse(remaining);
                        break;
                    case TokenClass.Operator:
                        if (Constituents.Count < 1 || (Constituents.Last() is SeparatorExpression))
                            return new Production(null, remaining);
                        current = new OperationExpression().Parse(Constituents.Last(), remaining);
                        if (current.Expression == null) break;
                        Constituents.RemoveAt(Constituents.Count - 1);
                        break;
                    default:
                        return new Production(null, remaining);
                }
                if (current.Expression == null)
                    return new Production(null, tokens);
                if (!(current.Expression is WhitespaceExpression)) 
                    Constituents.Add(current.Expression);
                remaining = current.RemainingTokens;
            }
            //parenthesis must be closed
            if (remaining.FirstOrDefault() == null || remaining.First().TokenClass != TokenClass.EndScope) return new Production(null, remaining);
            var separators = Constituents.Count(c => c is SeparatorExpression);
            var values = Constituents.Count(c => !(c is SeparatorExpression));
            //cannot be a separated expression without separators
            if (separators == 0) return new Production(null, tokens);
            //separated expressions require exactly n - 1 separators to values
            if (separators != (values - 1)) return new Production(null, tokens);
            var valuesOnly = Constituents.ToList().Aggregate(new List<Expression>(), (acc, i) =>
            {
                if (i is ScopeExpression || i is SeparatorExpression)
                    return acc;
                acc.Add(i);
                return acc;
            }).ToList();
            Constituents = valuesOnly;
            return new Production(this, remaining.Skip(1).ToList());
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values, FunctionSource functions) 
            => Constituents.Select(c => c.Evaluate(numericPrecision, values, functions)).ToList();
    }
}