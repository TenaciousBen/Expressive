using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Core.Language.Interpreter;

namespace Expressive.Core.Language.Expressions
{
    public class StringExpression : TerminatingExpression
    {
        public override Production Parse(List<Token> tokens)
        {
            var first = tokens.First();
            if (first.TokenClass != TokenClass.String)
                return new Production(null, tokens);
            Token = first;
            return new Production(this, tokens.ToList().Skip(1).ToList());
        }

        public string WithoutQuotes()
        {
            if (Token == null || String.IsNullOrEmpty(Token.Lexeme))
                return "";
            if (Token.Lexeme.Length < 2)
                return Token.Lexeme;
            return Token.Lexeme.Substring(1, Token.Lexeme.Length - 2);
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values,
            FunctionSource functions)
            => WithoutQuotes();
    }
}
