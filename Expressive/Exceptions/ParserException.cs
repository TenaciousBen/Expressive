using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apirion.Expressive.Core.Language.Expressions;

namespace Apirion.Expressive.Core.Exceptions
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
