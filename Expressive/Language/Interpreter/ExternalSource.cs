using System.Collections;
using System.Collections.Generic;
using Expressive.Core.Exceptions;

namespace Expressive.Core.Language.Interpreter
{
    public abstract class ExternalSource<T> : Dictionary<string, T>
    {
        public bool TryAddValue(string symbol, T value)
        {
            if (ContainsKey(symbol)) return false;
            Add(symbol, value);
            return true;
        }

        public T TryGetValue(string symbol)
        {
            if (!ContainsKey(symbol)) throw new UnmatchedSymbolException(symbol);
            return this[symbol];
        }
    }
}
