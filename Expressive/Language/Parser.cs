using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Core.Exceptions;
using Expressive.Core.Extensions;
using Expressive.Core.Language.Expressions;

namespace Expressive.Core.Language
{
    public static class Parser
    {
        //constant:
        //    Integer
        //    Float	
        //    String
        //    Boolean

        //symbol:
        //    ReplacementSymbol
        //    Symbol

        //value:
        //    constant
        //    symbol
        //    operation
        //    scope

        //operation: value Operator value

        //list-expression: value-source , value-source

        //scope: StartScope value-source EndScope

        //value-source: 
        //    value
        //    scope
        //    function
        //    operation

        //function: symbol StartScope list-expression EndScope | symbol StartScope value-source EndScope
        public static Expression Parse(List<Token> tokens)
        {
            if (tokens == null || tokens.None())
                return new NullExpression();
            var remainder = tokens.ToList();
            Expression program = null;
            while (remainder.Any())
            {
                Production parsed = null;
                var current = remainder.First();
                switch (current.TokenClass)
                {
                    case TokenClass.StartScope:
                        parsed = new ScopedExpression().Parse(remainder);
                        if (parsed.Expression != null) break;
                        parsed = new SeparatedExpression().Parse(remainder);
                        if (parsed.Expression != null) break;
                        parsed = new ErrorExpression().Parse(program, remainder);
                        break;
                    case TokenClass.Whitespace:
                        parsed = new WhitespaceExpression().Parse(remainder);
                        break;
                    case TokenClass.Symbol:
                        parsed = new OperationExpression().Parse(remainder);
                        if (parsed.Expression != null) break;
                        parsed = new FunctionExpression().Parse(remainder);
                        if (parsed.Expression != null) break;
                        parsed = new SymbolExpression().Parse(remainder);
                        if (parsed.Expression != null) break;
                        parsed = new ErrorExpression().Parse(program, remainder);
                        break;
                    case TokenClass.ReplacementSymbol:
                        parsed = new OperationExpression().Parse(remainder);
                        if (parsed.Expression != null) break;
                        parsed = new ReplacementSymbolExpression().Parse(remainder);
                        break;
                    case TokenClass.Integer:
                        parsed = new OperationExpression().Parse(remainder);
                        if (parsed.Expression != null) break;
                        parsed = new IntegerExpression().Parse(remainder);
                        break;
                    case TokenClass.Float:
                        parsed = new OperationExpression().Parse(remainder);
                        if (parsed.Expression != null) break;
                        parsed = new FloatExpression().Parse(remainder);
                        break;
                    case TokenClass.String:
                        parsed = new OperationExpression().Parse(remainder);
                        if (parsed.Expression != null) break;
                        parsed = new StringExpression().Parse(remainder);
                        break;
                    case TokenClass.Boolean:
                        parsed = new OperationExpression().Parse(remainder);
                        if (parsed.Expression != null) break;
                        parsed = new BooleanExpression().Parse(remainder);
                        break;
                    case TokenClass.Operator:
                        if (program == null) throw new ParserException(remainder);
                        parsed = new OperationExpression().Parse(program, remainder);
                        if (parsed.Expression != null) break;
                        parsed = new ErrorExpression().Parse(program, remainder);
                        break;
                    case TokenClass.Error:
                        throw new Exception("Unexpected sequence: " + remainder.First().Lexeme);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (parsed.Expression != null && !(parsed.Expression is WhitespaceExpression)) program = parsed.Expression;
                remainder = parsed.RemainingTokens;
            }
            if (program == null || remainder.Any()) throw new ParserException(remainder.Select(c => c.Lexeme).StringConcat());
            return program;
        }
    }
}
