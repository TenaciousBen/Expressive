using System.Text.RegularExpressions;
using Apirion.Expressive.Core.Language;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Apirion.Expressive.Tests.LexerTests
{
    [TestClass]
    public class LexerRegexTests
    {
        [TestMethod]
        public void SymbolRegexMatches()
        {
            const string singleSymbol = "x";
            const string multiSymbol = "function";
            const string alphaNumericSymbol = "function123";
            const string splitSymbol = "hi there";
            const string invalidPrependSymbol = "+function123";
            Assert.IsTrue(Lexer.SymbolRegex.IsMatch(singleSymbol));
            Assert.IsTrue(Lexer.SymbolRegex.IsMatch(multiSymbol));
            Assert.IsTrue(Lexer.SymbolRegex.IsMatch(alphaNumericSymbol));
            Match splitSymbolMatch = Lexer.SymbolRegex.Match(splitSymbol);
            Assert.AreEqual("hi", splitSymbolMatch.Value);
            Assert.IsFalse(Lexer.SymbolRegex.IsMatch(invalidPrependSymbol));
        }

        [TestMethod]
        public void FunctionRegexMatches()
        {
            const string parameterlessFunction = "function()";
            const string oneParamFunction = "function(1.0)";
            const string twoParamFunction = "function(1.0, 5)";
            const string alphanumericParamFunction = "function(12, [kitty])";
            const string invalidFunction = "function";
            const string invalidSpacedFunction = "function ()";
            const string invalidReversedFunction = "() function";
            const string invalidUnclosedFunction = "function(";
            const string invalidUnopenedFunction = "function)";
            const string invalidParameterOnlyFunction = "()";
            const string invalidParameterOnlyWithContentFunction = "(1, 2)";
            const string invalidNonStart = "1 + " + alphanumericParamFunction;
            Assert.IsTrue(Lexer.FunctionRegex.IsMatch(parameterlessFunction));
            Assert.IsTrue(Lexer.FunctionRegex.IsMatch(oneParamFunction));
            Assert.IsTrue(Lexer.FunctionRegex.IsMatch(twoParamFunction));
            Assert.IsTrue(Lexer.FunctionRegex.IsMatch(alphanumericParamFunction));
            Assert.IsFalse(Lexer.FunctionRegex.IsMatch(invalidFunction));
            Assert.IsFalse(Lexer.FunctionRegex.IsMatch(invalidSpacedFunction));
            Assert.IsFalse(Lexer.FunctionRegex.IsMatch(invalidReversedFunction));
            Assert.IsFalse(Lexer.FunctionRegex.IsMatch(invalidUnclosedFunction));
            Assert.IsFalse(Lexer.FunctionRegex.IsMatch(invalidUnopenedFunction));
            Assert.IsFalse(Lexer.FunctionRegex.IsMatch(invalidParameterOnlyFunction));
            Assert.IsFalse(Lexer.FunctionRegex.IsMatch(invalidParameterOnlyWithContentFunction));
            Assert.IsFalse(Lexer.FunctionRegex.IsMatch(invalidNonStart));
            Assert.IsFalse(Lexer.FunctionRegex.IsMatch(""));
            Assert.IsFalse(Lexer.FunctionRegex.IsMatch(" "));
        }

        [TestMethod]
        public void ReplacementSymbolRegexMatches()
        {
            const string alphaSymbol = "[symbol]";
            const string spacedAlphaSymbol = "[symbol one]";
            const string alphanumericSymbol = "[symbol123]";
            const string spacedAlphanumericSymbol = "[symbol 123]";
            const string specialSymbol = "[salary %]";
            const string salaryPrefixSymbol = "[salary], [bonus]";
            const string invalidEmptySymbol = "[]";
            const string invalidNonStart = "1 + " + specialSymbol;
            Assert.IsTrue(Lexer.ReplacementSymbolRegex.IsMatch(alphaSymbol));
            Assert.IsTrue(Lexer.ReplacementSymbolRegex.IsMatch(spacedAlphaSymbol));
            Assert.IsTrue(Lexer.ReplacementSymbolRegex.IsMatch(alphanumericSymbol));
            Assert.IsTrue(Lexer.ReplacementSymbolRegex.IsMatch(spacedAlphanumericSymbol));
            Assert.IsTrue(Lexer.ReplacementSymbolRegex.IsMatch(specialSymbol));
            Match salaryPrefixSymbolMatch = Lexer.ReplacementSymbolRegex.Match(salaryPrefixSymbol);
            Assert.AreEqual("[salary]", salaryPrefixSymbolMatch.Value);
            Assert.IsFalse(Lexer.ReplacementSymbolRegex.IsMatch(invalidEmptySymbol));
            Assert.IsFalse(Lexer.ReplacementSymbolRegex.IsMatch(invalidNonStart));
            Assert.IsFalse(Lexer.ReplacementSymbolRegex.IsMatch(""));
            Assert.IsFalse(Lexer.ReplacementSymbolRegex.IsMatch(" "));
        }

        [TestMethod]
        public void NumericMatches()
        {
            const string integer = "1";
            const string manyInteger = "12345";
            const string signedInteger = "-24";
            const string singleFloat = "1.0";
            const string longFloat = "12314.2312551";
            const string signedFloat = "-232.123";
            const string positiveFloat = "+123.32";
            const string invalidAlpha = "asd";
            const string invalidAlphanumeric = "asd312";
            const string invalidAlphafloat = "dqs32.1";
            const string invalidHalfFloat = ".10";
            const string invalidSignedHalfFloat = "+.3";
            Assert.IsTrue(Lexer.NumericRegex.IsMatch(integer));
            Assert.IsTrue(Lexer.NumericRegex.IsMatch(manyInteger));
            Assert.IsTrue(Lexer.NumericRegex.IsMatch(signedInteger));
            Assert.IsTrue(Lexer.NumericRegex.IsMatch(singleFloat));
            Assert.IsTrue(Lexer.NumericRegex.IsMatch(longFloat));
            Assert.IsTrue(Lexer.NumericRegex.IsMatch(signedFloat));
            Assert.IsTrue(Lexer.NumericRegex.IsMatch(positiveFloat));
            Assert.IsFalse(Lexer.NumericRegex.IsMatch(invalidAlpha));
            Assert.IsFalse(Lexer.NumericRegex.IsMatch(invalidAlphanumeric));
            Assert.IsFalse(Lexer.NumericRegex.IsMatch(invalidAlphafloat));
            Assert.IsFalse(Lexer.NumericRegex.IsMatch(invalidHalfFloat));
            Assert.IsFalse(Lexer.NumericRegex.IsMatch(invalidSignedHalfFloat));
        }

        [TestMethod]
        public void FloatMatches()
        {
            const string singleFloat = "1.0";
            const string longFloat = "12314.2312551";
            const string signedFloat = "-232.123";
            const string positiveFloat = "+123.32";
            const string invalidInteger = "1";
            const string invalidManyInteger = "12345";
            const string invalidSignedInteger = "-24";
            const string invalidAlpha = "asd";
            const string invalidAlphanumeric = "asd312";
            const string invalidAlphafloat = "dqs32.1";
            const string invalidHalfFloat = ".10";
            const string invalidSignedHalfFloat = "+.3";
            Assert.IsTrue(Lexer.FloatRegex.IsMatch(singleFloat));
            Assert.IsTrue(Lexer.FloatRegex.IsMatch(longFloat));
            Assert.IsTrue(Lexer.FloatRegex.IsMatch(signedFloat));
            Assert.IsTrue(Lexer.FloatRegex.IsMatch(positiveFloat));
            Assert.IsFalse(Lexer.FloatRegex.IsMatch(invalidInteger));
            Assert.IsFalse(Lexer.FloatRegex.IsMatch(invalidManyInteger));
            Assert.IsFalse(Lexer.FloatRegex.IsMatch(invalidSignedInteger));
            Assert.IsFalse(Lexer.FloatRegex.IsMatch(invalidAlpha));
            Assert.IsFalse(Lexer.FloatRegex.IsMatch(invalidAlphanumeric));
            Assert.IsFalse(Lexer.FloatRegex.IsMatch(invalidAlphafloat));
            Assert.IsFalse(Lexer.FloatRegex.IsMatch(invalidHalfFloat));
            Assert.IsFalse(Lexer.FloatRegex.IsMatch(invalidSignedHalfFloat));
        }

        [TestMethod]
        public void IntegerRegexMatches()
        {
            const string singleInt = "1";
            const string multiInt = "12";
            const string longInt = "1234212312";
            const string alphaNumeric = "123aSDsa";
            const string invalidAlpha = "asdasd";
            const string invalidAlphaNumeric = "asdas12312";
            string invalidNonStart = string.Format("func({0})", longInt);
            Assert.IsTrue(Lexer.IntegerRegex.IsMatch(singleInt));
            Assert.IsTrue(Lexer.IntegerRegex.IsMatch(multiInt));
            Assert.IsTrue(Lexer.IntegerRegex.IsMatch(longInt));
            Assert.IsTrue(Lexer.IntegerRegex.IsMatch(alphaNumeric));
            Assert.IsFalse(Lexer.IntegerRegex.IsMatch(invalidAlpha));
            Assert.IsFalse(Lexer.IntegerRegex.IsMatch(invalidAlphaNumeric));
            Assert.IsFalse(Lexer.IntegerRegex.IsMatch(invalidNonStart));
            Assert.IsFalse(Lexer.IntegerRegex.IsMatch(""));
            Assert.IsFalse(Lexer.IntegerRegex.IsMatch(" "));
        }

        [TestMethod]
        public void StringRegexMatches()
        {
            const string singleString = "\"String\"";
            const string emptyString = "\"\"";
            const string spacedString = "\"  \"";
            const string substring = "\"substring\"";
            const string substringWithRemainder = substring + "more string\"";
            const string alternateStyle = "'alternate'";
            const string mixedStyles = "'the inner string is \"foo\" you see?'";
            const string invalidNoQuotes = "string without quotes";
            const string invalidPrepend = "123'string'";
            const string invalidUnclosed = "'unclosed";
            Assert.IsTrue(Lexer.StringRegex.IsMatch(singleString));
            Assert.IsTrue(Lexer.StringRegex.IsMatch(emptyString));
            Assert.IsTrue(Lexer.StringRegex.IsMatch(spacedString));
            Match substringMatch = Lexer.StringRegex.Match(substringWithRemainder);
            Assert.AreEqual(substring, substringMatch.Value);
            Assert.IsTrue(Lexer.StringRegex.IsMatch(alternateStyle));
            var mixedStylesMatch = Lexer.StringRegex.Match(mixedStyles);
            Assert.AreEqual(mixedStyles, mixedStylesMatch.Value);
            Assert.IsFalse(Lexer.StringRegex.IsMatch(invalidNoQuotes));
            Assert.IsFalse(Lexer.StringRegex.IsMatch(invalidPrepend));
            Assert.IsFalse(Lexer.StringRegex.IsMatch(invalidUnclosed));
        }

        [TestMethod]
        public void WhitespaceRegexMatches()
        {
            const string newline = "\n";
            const string crlf = "\r\n";
            const string spaces = "     ";
            const string tabs = "\t\t";
            const string mixture = "  \t\r\n  \r\n";
            const string oneSpace = " The quick brown fox jumped over the lazy dog.";
            const string crlfPrepend = "\r\nThe quick brown fox jumped over the lazy dog.";
            const string invalidPrepend = "The quick brown fox jumped over the lazy dog.";
            Assert.IsTrue(Lexer.WhitespaceRegex.IsMatch(newline));
            Assert.IsTrue(Lexer.WhitespaceRegex.IsMatch(crlf));
            Assert.IsTrue(Lexer.WhitespaceRegex.IsMatch(spaces));
            Assert.IsTrue(Lexer.WhitespaceRegex.IsMatch(tabs));
            Match mixtureMatch = Lexer.WhitespaceRegex.Match(mixture);
            Assert.AreEqual(mixture, mixtureMatch.Value);
            Match oneSpaceMatch = Lexer.WhitespaceRegex.Match(oneSpace);
            Assert.AreEqual(" ", oneSpaceMatch.Value);
            Match crlfPrependMatch = Lexer.WhitespaceRegex.Match(crlfPrepend);
            Assert.AreEqual("\r\n", crlfPrependMatch.Value);
            Assert.IsFalse(Lexer.WhitespaceRegex.IsMatch(invalidPrepend));
        }

        [TestMethod]
        public void OperatorRegexMatches()
        {
            const string minus = "-";
            const string plus = "+";
            const string multiply = "*";
            const string divide = "/";
            const string equals = "=";
            const string notEquals = "!=";
            const string lessThan = "<";
            const string moreThan = ">";
            const string lessThanOrEqual = "<=";
            const string moreThanOrEqual = "!=";
            const string greaterThanMixture = ">=!=*=";
            const string prefixPlus = "+ 1 2";
            const string prefixNotEquals = "!= 1 1";
            const string invalidPrefix = "1 + 2";
            Assert.IsTrue(Lexer.OperatorRegex.IsMatch(minus));
            Assert.IsTrue(Lexer.OperatorRegex.IsMatch(plus));
            Assert.IsTrue(Lexer.OperatorRegex.IsMatch(multiply));
            Assert.IsTrue(Lexer.OperatorRegex.IsMatch(divide));
            Assert.IsTrue(Lexer.OperatorRegex.IsMatch(equals));
            Assert.IsTrue(Lexer.OperatorRegex.IsMatch(notEquals));
            Assert.IsTrue(Lexer.OperatorRegex.IsMatch(lessThan));
            Assert.IsTrue(Lexer.OperatorRegex.IsMatch(moreThan));
            Assert.IsTrue(Lexer.OperatorRegex.IsMatch(lessThanOrEqual));
            Assert.IsTrue(Lexer.OperatorRegex.IsMatch(moreThanOrEqual));
            Match prefixPlusMatch = Lexer.OperatorRegex.Match(prefixPlus);
            Assert.AreEqual("+", prefixPlusMatch.Value);
            Match prefixNotEqualsMatch = Lexer.OperatorRegex.Match(prefixNotEquals);
            Assert.AreEqual("!=", prefixNotEqualsMatch.Value);
            Match greaterThanMixtureMatch = Lexer.OperatorRegex.Match(greaterThanMixture);
            Assert.AreEqual(">=", greaterThanMixtureMatch.Value);
            Assert.IsFalse(Lexer.OperatorRegex.IsMatch(invalidPrefix));
        }

        [TestMethod]
        public void BooleanMatches()
        {
            const string lowerCaseTrue = "true";
            const string upperCaseTrue = "TRUE";
            const string lowerCaseFalse = "false";
            const string upperCaseFalse = "FALSE";
            const string mixedCaseTrue = "TrUe";
            const string mixedCaseFalse = "FaLsE";
            const string trueFalse = "true false";
            const string invalidPrepend = "+true";
            const string invalidMixed = "tru!e";
            Assert.IsTrue(Lexer.BooleanRegex.IsMatch(lowerCaseTrue));
            Assert.IsTrue(Lexer.BooleanRegex.IsMatch(upperCaseTrue));
            Assert.IsTrue(Lexer.BooleanRegex.IsMatch(lowerCaseFalse));
            Assert.IsTrue(Lexer.BooleanRegex.IsMatch(upperCaseFalse));
            Assert.IsTrue(Lexer.BooleanRegex.IsMatch(mixedCaseTrue));
            Assert.IsTrue(Lexer.BooleanRegex.IsMatch(mixedCaseFalse));
            Match trueFalseMatch = Lexer.BooleanRegex.Match(trueFalse);
            Assert.AreEqual("true", trueFalseMatch.Value);
            Assert.IsFalse(Lexer.BooleanRegex.IsMatch(invalidPrepend));
            Assert.IsFalse(Lexer.BooleanRegex.IsMatch(invalidMixed));
        }
    }
}