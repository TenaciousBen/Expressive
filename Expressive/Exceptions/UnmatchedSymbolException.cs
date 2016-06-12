using System;

namespace Expressive.Core.Exceptions
{
    public class UnmatchedSymbolException : Exception
    {
        public UnmatchedSymbolException(string symbol, Exception innerException) 
            : base($"Cannot match symbol '{symbol}' to any known function or value", innerException)
        {
        }

        public UnmatchedSymbolException(string symbol) : this(symbol, null)
        {
            
        }
    }
}
