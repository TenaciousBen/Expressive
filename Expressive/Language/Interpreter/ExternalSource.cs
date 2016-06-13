using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (string.IsNullOrEmpty(symbol)) throw new ArgumentNullException(nameof(symbol));
            var caseInsensitiveKey = Keys.FirstOrDefault(k => k.ToLower() == symbol.ToLower());
            if (string.IsNullOrEmpty(caseInsensitiveKey)) throw new UnmatchedSymbolException(symbol);
            return this[caseInsensitiveKey];
        }
    }
}
