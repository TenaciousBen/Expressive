using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apirion.Expressive.Core.Language;

namespace Apirion.Expressive.Core.Exceptions
{
    public class LexerException : Exception
    {
        public List<Token> ParsedTokens { get; set; }
        public string Remaining { get; set; }
        public int Index { get; set; }
        public string Input { get; set; }

        public LexerException(List<Token> parsedTokens, string remaining, string input, int index, Exception innerException)
            : base ($"Lexing failed at index {index}, see contained properties for details", innerException)
        {
            ParsedTokens = parsedTokens;
            Remaining = remaining;
            Index = index;
            Input = input;
        }

        public LexerException(List<Token> parsedTokens, string remaining, string input, int index)
            : this(parsedTokens, remaining, input, index, null)
        {
        }
    }
}
