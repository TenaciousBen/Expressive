using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Expressive.Core.Language;

namespace Expressive.Core.Exceptions
{
    public class ParserException : Exception
    {
        public string RemainingLexemes { get; set; }

        public ParserException(string remainingLexemes, Exception innerException) 
            : base($"Could not parse the following expression: {remainingLexemes}", innerException)
        {
            RemainingLexemes = remainingLexemes;
        }

        public ParserException(string remainingLexemes)
            : this(remainingLexemes, null)
        {
        }

        public ParserException(IEnumerable<Token> remainingLexemes)
            : this(string.Join("", remainingLexemes?.Select(l => l?.Lexeme ?? "") ?? new List<string>()))
        {
            
        }
    }
}
