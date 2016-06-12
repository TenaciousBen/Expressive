using System.Collections;
using System.Collections.Generic;
using Expressive.Core.Exceptions;

namespace Expressive.Core.Language.Interpreter
{
    public abstract class ExternalSource<T> : IEnumerable<KeyValuePair<string, T>>
    {
        protected Dictionary<string, T> Values { get; set; }

        public ExternalSource()
        {
            Values = new Dictionary<string, T>();
        }
        public bool TryAddValue(string symbol, T value)
        {
            if (Values.ContainsKey(symbol)) return false;
            Values.Add(symbol, value);
            return true;
        }

        public T TryGetValue(string symbol)
        {
            if (!Values.ContainsKey(symbol)) throw new UnmatchedSymbolException(symbol);
            return Values[symbol];
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Values.GetEnumerator();
        }
    }
}
