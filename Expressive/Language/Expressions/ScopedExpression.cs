using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Apirion.Expressive.Core.Extensions;
using Apirion.Expressive.Core.Language.Interpreter;

namespace Apirion.Expressive.Core.Language.Expressions
{
    public class ScopedExpression : Expression
    {
        public override string ToString() { return "(" + Constituents.StringConcat() + ")"; }

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
                        current = new OperationExpression().Parse(remaining);
                        if (current.Expression != null) break;
                        if (Constituents.Count > 0) return new Production(null, tokens); //cannot have multiple value types in a row
                        current = new ReplacementSymbolExpression().Parse(remaining);
                        break;
                    case TokenClass.Integer:
                        current = new OperationExpression().Parse(remaining);
                        if (current.Expression != null) break;
                        if (Constituents.Count > 0) return new Production(null, tokens);
                        current = new IntegerExpression().Parse(remaining);
                        break;
                    case TokenClass.Float:
                        current = new OperationExpression().Parse(remaining);
                        if (current.Expression != null) break;
                        if (Constituents.Count > 0) return new Production(null, tokens);
                        current = new FloatExpression().Parse(remaining);
                        break;
                    case TokenClass.String:
                        current = new OperationExpression().Parse(remaining);
                        if (current.Expression != null) break;
                        if (Constituents.Count > 0) return new Production(null, tokens);
                        current = new StringExpression().Parse(remaining);
                        break;
                    case TokenClass.Boolean:
                        current = new OperationExpression().Parse(remaining);
                        if (current.Expression != null) break;
                        if (Constituents.Count > 0) return new Production(null, tokens);
                        current = new BooleanExpression().Parse(remaining);
                        break;
                    case TokenClass.Symbol:
                        current = new FunctionExpression().Parse(remaining);
                        if (current.Expression != null) break;
                        if (Constituents.Count > 0) return new Production(null, tokens);
                        current = new SymbolExpression().Parse(remaining);
                        break;
                    case TokenClass.StartScope:
                        current = new SeparatedExpression().Parse(remaining);
                        if (current.Expression != null) break;
                        if (Constituents.Count > 0) return new Production(null, tokens);
                        current = new ScopedExpression().Parse(remaining);
                        break;
                    case TokenClass.Whitespace:
                        current = new WhitespaceExpression().Parse(remaining);
                        break;
                    case TokenClass.Operator:
                        if (Constituents.Count != 1)
                            return new Production(null, remaining);
                        current = new OperationExpression().Parse(Constituents.First(), remaining);
                        if (current.Expression == null) break;
                        Constituents.Clear();
                        break;
                    default:
                        return new Production(null, remaining);
                        break;
                }
                if (current.Expression == null)
                    return new Production(null, tokens);
                if (!(current.Expression is WhitespaceExpression))
                    Constituents.Add(current.Expression);
                remaining = current.RemainingTokens;
            }
            if (remaining.None()) return new Production(null, tokens);
            if (remaining[0].TokenClass != TokenClass.EndScope) return new Production(null, tokens);
            return new Production(this, remaining.Skip(1).ToList());
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values, FunctionSource functions)
            => Constituents[0].Evaluate(numericPrecision, values, functions);
    }
}