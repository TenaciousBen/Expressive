using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apirion.Expressive.Core.Exceptions
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
