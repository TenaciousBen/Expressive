using System.Collections.Generic;
using Expressive.Core.Language;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Expressive.Tests.LexerTests
{
    [TestClass]
    public class LexingTests
    {
        [TestMethod]
        public void CanLexBasicReplacementSymbols()
        {
            var expression = "([age], [weight])";
            var expected = new List<Token>
                {
                    new Token(TokenClass.StartScope, "("),
                    new Token(TokenClass.ReplacementSymbol, "[age]"),
                    new Token(TokenClass.Separator, ","),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.ReplacementSymbol, "[weight]"),
                    new Token(TokenClass.EndScope, ")")
                };
            var lexed = Lexer.Lex(expression);
            Assert.AreEqual(expected.Count, lexed.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                var expectedToken = expected[i];
                var actualToken = lexed[i];
                Assert.AreEqual(expectedToken.TokenClass, actualToken.TokenClass);
                Assert.AreEqual(expectedToken.Lexeme, actualToken.Lexeme);
            }
        }

        [TestMethod]
        public void CanLexBasicStringSymbols()
        {
            var expression = "('height', 'weight')";
            var expected = new List<Token>
                {
                    new Token(TokenClass.StartScope, "("),
                    new Token(TokenClass.String, "'height'"),
                    new Token(TokenClass.Separator, ","),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.String, "'weight'"),
                    new Token(TokenClass.EndScope, ")")
                };
            var lexed = Lexer.Lex(expression);
            Assert.AreEqual(expected.Count, lexed.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                var expectedToken = expected[i];
                var actualToken = lexed[i];
                Assert.AreEqual(expectedToken.TokenClass, actualToken.TokenClass);
                Assert.AreEqual(expectedToken.Lexeme, actualToken.Lexeme);
            }
        }

        [TestMethod]
        public void CanLexBasicOperatorSymbols()
        {
            var expression = "([new height] - [height]) = [increase %]";
            var expected = new List<Token>
                {
                    new Token(TokenClass.StartScope, "("),
                    new Token(TokenClass.ReplacementSymbol, "[new height]"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "-"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.ReplacementSymbol, "[height]"),
                    new Token(TokenClass.EndScope, ")"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "="),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.ReplacementSymbol, "[increase %]")
                };
            var lexed = Lexer.Lex(expression);
            Assert.AreEqual(expected.Count, lexed.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                var expectedToken = expected[i];
                var actualToken = lexed[i];
                Assert.AreEqual(expectedToken.TokenClass, actualToken.TokenClass);
                Assert.AreEqual(expectedToken.Lexeme, actualToken.Lexeme);
            }
        }

        [TestMethod]
        public void CanLexIntegerAndFloatNumerics()
        {
            var expression = "((100 / 33.33) * 33)";
            var expected = new List<Token>
                {
                    new Token(TokenClass.StartScope, "("),
                    new Token(TokenClass.StartScope, "("),
                    new Token(TokenClass.Integer, "100"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "/"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Float, "33.33"),
                    new Token(TokenClass.EndScope, ")"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "*"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Integer, "33"),
                    new Token(TokenClass.EndScope, ")")
                };
            var lexed = Lexer.Lex(expression);
            Assert.AreEqual(expected.Count, lexed.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                var expectedToken = expected[i];
                var actualToken = lexed[i];
                Assert.AreEqual(expectedToken.TokenClass, actualToken.TokenClass);
                Assert.AreEqual(expectedToken.Lexeme, actualToken.Lexeme);
            }
        }

        [TestMethod]
        public void CanLexBoolean()
        {
            var expression = "[new height] > [height] = true";
            var expected = new List<Token>
                {
                    new Token(TokenClass.ReplacementSymbol, "[new height]"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, ">"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.ReplacementSymbol, "[height]"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "="),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Boolean, "true")
                };
            var lexed = Lexer.Lex(expression);
            Assert.AreEqual(expected.Count, lexed.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                var expectedToken = expected[i];
                var actualToken = lexed[i];
                Assert.AreEqual(expectedToken.TokenClass, actualToken.TokenClass);
                Assert.AreEqual(expectedToken.Lexeme, actualToken.Lexeme);
            }
        }

        [TestMethod]
        public void CanLexSymbols()
        {
            var expression = "AND(((([new height] / [height]) * 100) - 100) < 3, AND([attribute] > 2, [attribute] < 4))";
            var expected = new List<Token>
                {
                    new Token(TokenClass.Symbol, "AND"),
                    new Token(TokenClass.StartScope, "("),
                    new Token(TokenClass.StartScope, "("),
                    new Token(TokenClass.StartScope, "("),
                    new Token(TokenClass.StartScope, "("),
                    new Token(TokenClass.ReplacementSymbol, "[new height]"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "/"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.ReplacementSymbol, "[height]"),
                    new Token(TokenClass.EndScope, ")"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "*"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Integer, "100"),
                    new Token(TokenClass.EndScope, ")"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "-"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Integer, "100"),
                    new Token(TokenClass.EndScope, ")"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "<"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Integer, "3"),
                    new Token(TokenClass.Separator, ","),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Symbol, "AND"),
                    new Token(TokenClass.StartScope, "("),
                    new Token(TokenClass.ReplacementSymbol, "[attribute]"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, ">"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Integer, "2"),
                    new Token(TokenClass.Separator, ","),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.ReplacementSymbol, "[attribute]"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "<"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Integer, "4"),
                    new Token(TokenClass.EndScope, ")"),
                    new Token(TokenClass.EndScope, ")")
                };
            var lexed = Lexer.Lex(expression);
            Assert.AreEqual(expected.Count, lexed.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                var expectedToken = expected[i];
                var actualToken = lexed[i];
                Assert.AreEqual(expectedToken.TokenClass, actualToken.TokenClass);
                Assert.AreEqual(expectedToken.Lexeme, actualToken.Lexeme);
            }
        }

        [TestMethod]
        public void NewlinesAndTabsAreLexed()
        {
            var expression = "+\r\n\t1\t2\t3.5\r\n\r\ndone";
            var expected = new List<Token>
                {
                    new Token(TokenClass.Operator, "+"),
                    new Token(TokenClass.Whitespace, "\r\n\t"),
                    new Token(TokenClass.Integer, "1"),
                    new Token(TokenClass.Whitespace, "\t"),
                    new Token(TokenClass.Integer, "2"),
                    new Token(TokenClass.Whitespace, "\t"),
                    new Token(TokenClass.Float, "3.5"),
                    new Token(TokenClass.Whitespace, "\r\n\r\n"),
                    new Token(TokenClass.Symbol, "done"),
                };
            var lexed = Lexer.Lex(expression);
            Assert.AreEqual(expected.Count, lexed.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                var expectedToken = expected[i];
                var actualToken = lexed[i];
                Assert.AreEqual(expectedToken.TokenClass, actualToken.TokenClass);
                Assert.AreEqual(expectedToken.Lexeme, actualToken.Lexeme);
            }
        }

        [TestMethod]
        public void CanDifferentiateBoolAndSymbol()
        {
            var expression = "(false + isfalse) + truevalue = (true - false)";
            var expected = new List<Token>
                {
                    new Token(TokenClass.StartScope, "("),
                    new Token(TokenClass.Boolean, "false"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "+"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Symbol, "isfalse"),
                    new Token(TokenClass.EndScope, ")"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "+"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Symbol, "truevalue"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "="),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.StartScope, "("),
                    new Token(TokenClass.Boolean, "true"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Operator, "-"),
                    new Token(TokenClass.Whitespace, " "),
                    new Token(TokenClass.Boolean, "false"),
                    new Token(TokenClass.EndScope, ")")
                };
            var lexed = Lexer.Lex(expression);
            Assert.AreEqual(expected.Count, lexed.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                var expectedToken = expected[i];
                var actualToken = lexed[i];
                Assert.AreEqual(expectedToken.TokenClass, actualToken.TokenClass);
                Assert.AreEqual(expectedToken.Lexeme, actualToken.Lexeme);
            }
        }
    }
}
