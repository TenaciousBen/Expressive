using System;

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
    }
}
