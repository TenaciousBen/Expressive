using System.Collections.Generic;
using System.Text.RegularExpressions;
using Apirion.Expressive.Core.Exceptions;

namespace Apirion.Expressive.Core.Language
{
    public static class Lexer
    {
        public readonly static char StartScopeChar = '(';
        public readonly static char EndScopeChar = ')';
        public readonly static char SeparatorChar = ',';
        //at least one space, newline, crlf, or tab
        public readonly static Regex WhitespaceRegex = new Regex(@"^[ |\n|\r|\t]+");
        //[content] where content can be anything except a ] 
        public readonly static Regex ReplacementSymbolRegex = new Regex(@"^(\[.+?\])");
        //Abc123 where the symbol starts with alpha
        public readonly static Regex SymbolRegex = new Regex(@"^[a-zA-Z]([a-zA-Z]|[0-9])*");
        //Abc123(content) as above, where content can be anything except )
        public readonly static Regex FunctionRegex = new Regex(@"^([a-zA-Z]([a-zA-Z]|[0-9])*)\(.*?\)"); 
        //Any single operator, where a larger operator takes precedent over a smaller e.g. == before =
        public readonly static Regex OperatorRegex = new Regex(@"^(!=|>=|<=|\+|-|/|\*|=|>|<){1}");
        //-14.5 where sign may be present, digts before dot must be present, and digts after dot must be present if dot is present
        public readonly static Regex NumericRegex = new Regex(@"^[-+]?([0-9]+\.[0-9]+|[0-9]+)");
        //-5 where sign may be present and digits must be present
        public readonly static Regex IntegerRegex = new Regex(@"^[+-]?[0-9]+");
        //-5.5 where sign may be present, digits must be present, dot must be present and digits after dot must be present
        public readonly static Regex FloatRegex = new Regex(@"^[-+]?([0-9]+\.[0-9]+)");
        //'content' where content may be enclosed by " or ' and may consist of any characters except its enclosing " or '
        public readonly static Regex StringRegex = new Regex("^(\".*?\")+|^('.*?')+");
        //true or false, where case doesn't matter and where the boolean must either have nothing, an endscope or a space after it
        public readonly static Regex BooleanRegex = new Regex(@"^((?i)true$|(?i)true(?= )|(?i)true(?=\))|(?i)false$|(?i)false(?= )|(?i)false(?=\)))");

        /// <summary>
        /// Lexes the input expression into a list of tokens
        /// </summary>
        /// <param name="expression">An expression of the Expressive language</param>
        /// <returns>A list of tokens</returns>
        public static List<Token> Lex(string expression)
        {
            var lexemes = new List<Token>();
            for (int i = 0; i < expression.Length; i++)
            {
                var character = expression[i];
                if (character == StartScopeChar)
                {
                    lexemes.Add(new Token(TokenClass.StartScope, character.ToString()));
                    continue;
                }
                if (character == EndScopeChar)
                {
                    lexemes.Add(new Token(TokenClass.EndScope, character.ToString()));
                    continue;
                }
                if (character == SeparatorChar)
                {
                    lexemes.Add(new Token(TokenClass.Separator, character.ToString()));
                    continue;
                }
                var remainder = expression.Substring(i);
                var whitespace = WhitespaceRegex.Match(remainder);
                if (whitespace.Success)
                {
                    lexemes.Add(new Token(TokenClass.Whitespace, whitespace.Value));
                    //when adding a potentially multi-char lexeme, skip ahead in the expression past the end of the lexeme
                    i += whitespace.Value.Length - 1;
                    continue;
                }
                var replacement = ReplacementSymbolRegex.Match(remainder);
                if (replacement.Success)
                {
                    lexemes.Add(new Token(TokenClass.ReplacementSymbol, replacement.Value));
                    i += replacement.Value.Length - 1;
                    continue;
                }
                var stringMatch = StringRegex.Match(remainder);
                if (stringMatch.Success)
                {
                    lexemes.Add(new Token(TokenClass.String, stringMatch.Value));
                    i += stringMatch.Value.Length - 1;
                    continue;
                }
                //evaluate numerics before operators, as a numeric can be composed of an operator and a number e.g. -5
                var numeric = NumericRegex.Match(remainder);
                if (numeric.Success)
                {
                    //if a value is numeric, its numeric type can be determined by seeing if it's float: if it's not float then it's int
                    var floatMatch = FloatRegex.Match(remainder);
                    if (floatMatch.Success)
                    {
                        lexemes.Add(new Token(TokenClass.Float, floatMatch.Value));
                        i += floatMatch.Value.Length - 1;
                    }
                    else
                    {
                        lexemes.Add(new Token(TokenClass.Integer, numeric.Value));
                        i += numeric.Value.Length - 1;
                    }
                    continue;
                }
                var operatorMatch = OperatorRegex.Match(remainder);
                if (operatorMatch.Success)
                {
                    lexemes.Add(new Token(TokenClass.Operator, operatorMatch.Value));
                    i += operatorMatch.Value.Length - 1;
                    continue;
                }
                //order evaluation by specificity: symbols are the least specific recognized lexeme, so keywords like bools are evaluated first
                //this prevents us from determining 'true' to be a symbol
                var boolean = BooleanRegex.Match(remainder);
                if (boolean.Success)
                {
                    lexemes.Add(new Token(TokenClass.Boolean, boolean.Value));
                    i += boolean.Value.Length - 1;
                    continue;
                }
                var symbol = SymbolRegex.Match(remainder);
                if (symbol.Success)
                {
                    lexemes.Add(new Token(TokenClass.Symbol, symbol.Value));
                    i += symbol.Value.Length - 1;
                    continue;
                }
                throw new LexerException(lexemes, remainder, expression, i);
            }
            return lexemes;
        }
    }
}
