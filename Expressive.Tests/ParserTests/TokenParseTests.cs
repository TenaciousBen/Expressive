﻿using System;
using Expressive.Core.Exceptions;
using Expressive.Core.Language;
using Expressive.Core.Language.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Expressive.Tests.ParserTests
{
    [TestClass]
    public class TokenParseTests
    {
        [TestMethod]
        public void OperatorOnlyExpressionsFailGracefully()
        {
            AssertParserFailsGracefully("+/-*");
            AssertParserFailsGracefully("+");
            AssertParserFailsGracefully("+++");
        }

        [TestMethod]
        public void CanParseSimpleScopes()
        {
            var expression = "(1)";
            var tokens = Lexer.Lex(expression);
            var parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(IntegerExpression));
            expression = "(symbol)";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(SymbolExpression));
            expression = "( 1.5 )";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(FloatExpression));
            expression = "(true)";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(BooleanExpression));
            expression = "(                         [age]\r\n\t)";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(ReplacementSymbolExpression));
        }

        [TestMethod]
        public void CanParseNestedScopes()
        {
            var expression = "((1))";
            var tokens = Lexer.Lex(expression);
            var parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[0], typeof(IntegerExpression));
            expression = "(((symbol)))";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[0], typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[0].Constituents[0], typeof(SymbolExpression));
            expression = "( ( ( ( 'string' ) ) ) )";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[0], typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[0].Constituents[0], typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[0].Constituents[0].Constituents[0], typeof(StringExpression));
        }

        [TestMethod]
        public void CanParseSequence()
        {
            var expression = "(1, 2, 3, 4)";
            var tokens = Lexer.Lex(expression);
            var parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(IntegerExpression));
            Assert.AreEqual("1", parsed.Constituents[0].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[1], typeof(IntegerExpression));
            Assert.AreEqual("2", parsed.Constituents[1].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[2], typeof(IntegerExpression));
            Assert.AreEqual("3", parsed.Constituents[2].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[3], typeof(IntegerExpression));
            Assert.AreEqual("4", parsed.Constituents[3].ToString());
            expression = "(1, 2, 3, (4, 5, 6))";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(IntegerExpression));
            Assert.AreEqual("1", parsed.Constituents[0].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[1], typeof(IntegerExpression));
            Assert.AreEqual("2", parsed.Constituents[1].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[2], typeof(IntegerExpression));
            Assert.AreEqual("3", parsed.Constituents[2].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[3], typeof(ScopedExpression));
            expression = "(1.52, symbol)";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(FloatExpression));
            Assert.AreEqual("1.52", parsed.Constituents[0].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[1], typeof(SymbolExpression));
            Assert.AreEqual("symbol", parsed.Constituents[1].ToString());
        }

        [TestMethod]
        public void CanParseFunction()
        {
            var expression = "function()";
            var tokens = Lexer.Lex(expression);
            var parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(SymbolExpression));
            Assert.AreEqual("function", parsed.Constituents[0].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[1], typeof(ScopedExpression));
            Assert.AreEqual("()", parsed.Constituents[1].ToString());
            expression = "splat([full name], ',')";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(SymbolExpression));
            Assert.AreEqual("splat", parsed.Constituents[0].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[1], typeof(SeparatedExpression));
            Assert.AreEqual("([full name], ',')", parsed.Constituents[1].ToString());
            expression = "concat(getFirstName([full name]), getRaise([height], [new height]))";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(SymbolExpression));
            Assert.AreEqual("concat", parsed.Constituents[0].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[1], typeof(SeparatedExpression));
            Assert.AreEqual("(getFirstName([full name]), getRaise([height], [new height]))", parsed.Constituents[1].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[1].Constituents[0], typeof(FunctionExpression));
            Assert.AreEqual("getFirstName([full name])", parsed.Constituents[1].Constituents[0].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[1].Constituents[1], typeof(FunctionExpression));
            Assert.AreEqual("getRaise([height], [new height])", parsed.Constituents[1].Constituents[1].ToString());
        }

        [TestMethod]
        public void CanParseOperation()
        {
            var expression = @"1 + 2";
            var tokens = Lexer.Lex(expression);
            var parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(OperationExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(IntegerExpression));
            Assert.AreEqual("1", parsed.Constituents[0].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[1], typeof(OperatorExpression));
            Assert.AreEqual("+", parsed.Constituents[1].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[2], typeof(IntegerExpression));
            Assert.AreEqual("2", parsed.Constituents[2].ToString());
            expression = @"1 + 2 - 3";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(OperationExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(IntegerExpression));
            Assert.AreEqual("1", parsed.Constituents[0].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[1], typeof(OperatorExpression));
            Assert.AreEqual("+", parsed.Constituents[1].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[2], typeof(OperationExpression));
            Assert.AreEqual("2-3", parsed.Constituents[2].ToString());
            expression = @"((1 + 2) - 3)";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(OperationExpression));
            Assert.AreEqual("((1+2)-3)", parsed.ToString());
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[0], typeof(ScopedExpression));
            Assert.AreEqual("(1+2)", parsed.Constituents[0].Constituents[0].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[0].Constituents[0], typeof(OperationExpression));
            Assert.AreEqual("1+2", parsed.Constituents[0].Constituents[0].Constituents[0].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[1], typeof(OperatorExpression));
            Assert.AreEqual("-", parsed.Constituents[0].Constituents[1].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[2], typeof(IntegerExpression));
            Assert.AreEqual("3", parsed.Constituents[0].Constituents[2].ToString());
            expression = @"((1 + 2) - (3 + 4 - (2 * 2)))";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ScopedExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(OperationExpression));
            Assert.AreEqual("((1+2)-(3+4-(2*2)))", parsed.ToString());
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[0], typeof(ScopedExpression));
            Assert.AreEqual("(1+2)", parsed.Constituents[0].Constituents[0].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[0].Constituents[0], typeof(OperationExpression));
            Assert.AreEqual("1+2", parsed.Constituents[0].Constituents[0].Constituents[0].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[1], typeof(OperatorExpression));
            Assert.AreEqual("-", parsed.Constituents[0].Constituents[1].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[0].Constituents[2], typeof(ScopedExpression));
            Assert.AreEqual("(3+4-(2*2))", parsed.Constituents[0].Constituents[2].ToString());
            expression = @"function(Square(1 + 2) - Sqrt(3 + 4 - (2 * 2)))";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(FunctionExpression));
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(SymbolExpression));
            Assert.IsInstanceOfType(parsed.Constituents[1], typeof(ScopedExpression));
            Assert.AreEqual("(Square(1+2)-Sqrt(3+4-(2*2)))", parsed.Constituents[1].ToString());
            Assert.IsInstanceOfType(parsed.Constituents[1].Constituents[0], typeof(OperationExpression));
            Assert.AreEqual("Square(1+2)", parsed.Constituents[1].Constituents[0].Constituents[0].ToString());
        }

        [TestMethod]
        public void CanParseComplexExpressions()
        {
            var expression = @"(([New height] - [height]) / [height]) * 100 > 3";
            var tokens = Lexer.Lex(expression);
            var parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(OperationExpression));
            Assert.AreEqual("(([New height]-[height])/[height])*100>3", parsed.ToString());
            expression = "And((([New height] - [height]) / [height]) * 100 > 3, (([New height] - [height]) / [height]) * 100 < 5)";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(FunctionExpression));
            Assert.AreEqual("And((([New height]-[height])/[height])*100>3, (([New height]-[height])/[height])*100<5)", parsed.ToString());
            expression = @"If(And((([New height] - [height]) / [height]) * 100 > 3, (([New height] - [height]) / [height]) * 100 < 5), [medicalHistory])";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(FunctionExpression));
            Assert.AreEqual("If(And((([New height]-[height])/[height])*100>3, (([New height]-[height])/[height])*100<5), [medicalHistory])", parsed.ToString());
            expression = @"(And((([New height] - [height]) / [height]) * 100 > 3, (([New height] - [height]) / [height]) * 100 < 5), [medicalHistory])";
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ScopedExpression));
            Assert.AreEqual("(And((([New height]-[height])/[height])*100>3, (([New height]-[height])/[height])*100<5), [medicalHistory])", parsed.ToString());
        }

        [TestMethod]
        public void AssertParserFailsGracefully()
        {
            var expression = @"(([New height] - [height]) / [height]"; // error: unclosed scope
            var tokens = Lexer.Lex(expression);
            var parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ErrorExpression));
            Assert.AreEqual(expression, parsed.ToString());
            expression = @"([New height] - [height]) / [height] *"; // error: [height] *
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ErrorExpression));
            Assert.AreEqual(parsed.ToString(), "*");
            Assert.IsInstanceOfType(parsed.Constituents[0], typeof(OperationExpression));
            Assert.AreEqual(parsed.Constituents[0].ToString(), "([New height]-[height])/[height]");
            expression = @"And((([New height][height])/[height])*100>3, (([New height]-[height])/[height])*100<5)"; // error: ([New height][height])
            tokens = Lexer.Lex(expression);
            parsed = Parser.Parse(tokens);
            Assert.IsInstanceOfType(parsed, typeof(ErrorExpression));
            Assert.AreEqual("((([New height][height])/[height])*100>3, (([New height]-[height])/[height])*100<5)", parsed.ToString());
        }

        private static void AssertParserFailsGracefully(string expression)
        {
            ParserException thrown = null;
            ErrorExpression error = null;
            try
            {
                var tokens = Lexer.Lex(expression);
                var parsed = Parser.Parse(tokens);
                error = parsed as ErrorExpression;
            }
            catch (ParserException e)
            {
                thrown = e;
            }
            if (error == null) Assert.IsNotNull(thrown);
            if (thrown == null) Assert.IsNotNull(error);
        }
    }
}
