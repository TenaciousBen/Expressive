using System;
using System.Collections.Generic;
using System.Linq;
using Apirion.Expressive.Core.Language;
using Apirion.Expressive.Core.Language.Expressions;

namespace Apirion.Expressive.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool ContainsAny<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if (first == null || second == null)
                return false;
            return second.Any(first.Contains);
        }

        public static string ToTokenString(this IEnumerable<Expression> expressions)
        {
            var eager = expressions == null ? new List<Expression>() : expressions.ToList();
            if (!eager.Any())
                return "";
            return eager.Select(e => e.Token.Lexeme).Aggregate("", (acc, i) => acc + i);
        }

        public static string StringConcat<T>(this IEnumerable<T> thisEnumerable)
        {
            if (thisEnumerable == null)
                return null;
            return thisEnumerable.Select(i => i.ToString()).Aggregate("", (acc, i) => acc + i);
        }

        public static bool None<T>(this IEnumerable<T> thisEnumerable, Func<T, bool> predicate = null)
        {
            var eager = thisEnumerable == null ? new List<T>() : thisEnumerable.ToList();
            if (predicate == null)
                return eager.Count == 0;
            return !eager.Any(predicate);
        }

        public static Token NextNonWhitespace(this IEnumerable<Token> tokens)
        {
            var nonWhitespace = tokens.FirstOrDefault(t => t.TokenClass != TokenClass.Whitespace);
            return nonWhitespace;
        }

        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> thisEnumerable, int count)
        {
            var eager = thisEnumerable.ToList();
            var length = eager.Count - count;
            return eager.Take(length);
        }

        public static IEnumerable<T> Rest<T>(this IEnumerable<T> thisEnumerable)
        {
            return thisEnumerable.Skip(1);
        }
    }
}
