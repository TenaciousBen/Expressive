using System;
using System.Collections.Generic;
using System.Linq;
using Expressive.Core.Exceptions;
using Expressive.Core.Extensions;
using Expressive.Core.Language.Interpreter;
using Expressive.Core.Language.Types;

namespace Expressive.Core.Language.Expressions
{
    public class OperationExpression : Expression
    {
        public OperatorExpression Operator
        {
            get { return (OperatorExpression)Constituents[1]; }
        }
        public Expression LeftOperand
        {
            get { return Constituents[0]; }
        }
        public Expression RightOperand
        {
            get { return Constituents[2]; }
        }

        public override Production Parse(List<Token> tokens)
        {
            var nonWhitespace = tokens.NextNonWhitespace();
            if (nonWhitespace.TokenClass != TokenClass.Boolean &&
                nonWhitespace.TokenClass != TokenClass.Float &&
                nonWhitespace.TokenClass != TokenClass.Integer &&
                nonWhitespace.TokenClass != TokenClass.ReplacementSymbol &&
                nonWhitespace.TokenClass != TokenClass.StartScope &&
                nonWhitespace.TokenClass != TokenClass.String &&
                nonWhitespace.TokenClass != TokenClass.Symbol)
                return new Production(null, tokens);
            var remainder = tokens.ToList();
            while (remainder.Any() && Constituents.Count < 3) // An operation must have exactly three items: value op value
            {
                var currentToken = remainder.First().TokenClass;
                Production current = null;
                //check whether the right hand side of the operation is another operation
                if (Constituents.Count == 2) current = new OperationExpression().Parse(remainder);
                if (Constituents.Count != 2 || current?.Expression == null)
                    switch (currentToken)
                    {
                        case TokenClass.StartScope:
                            current = new ScopedExpression().Parse(remainder);
                            break;
                        case TokenClass.Whitespace:
                            current = new WhitespaceExpression().Parse(remainder);
                            break;
                        case TokenClass.ReplacementSymbol:
                            current = new ReplacementSymbolExpression().Parse(remainder);
                            break;
                        case TokenClass.Symbol:
                            current = new FunctionExpression().Parse(remainder);
                            if (current.Expression == null) current = new SymbolExpression().Parse(remainder);
                            break;
                        case TokenClass.Operator:
                            if (Constituents.Count == 2)
                            {
                                current = NumericExpression.TryParseNumeric(remainder);
                                break;
                            }
                            current = new OperatorExpression().Parse(remainder);
                            break;
                        case TokenClass.Integer:
                        case TokenClass.Float:
                            current = NumericExpression.TryParseNumeric(remainder);
                            break;
                        case TokenClass.String:
                            current = new StringExpression().Parse(remainder);
                            break;
                        case TokenClass.Boolean:
                            current = new BooleanExpression().Parse(remainder);
                            break;
                        default:
                            return new Production(null, tokens);
                    }
                if (current == null || current.Expression == null) return new Production(null, tokens);
                remainder = current.RemainingTokens;
                if (!(current.Expression is WhitespaceExpression)) Constituents.Add(current.Expression);
            }
            if (Constituents.None(c => c is OperatorExpression)) return new Production(null, tokens);
            if (Constituents.Count != 3) return new Production(null, tokens);
            return new Production(this, remainder);
        }

        /// <summary>
        /// Creates an operation by combining an already parsed expression with the right side of an operation expression's tokens
        /// </summary>
        public Production Parse(Expression left, List<Token> tokens)
        {
            var nonWhitespace = tokens.NextNonWhitespace();
            if (nonWhitespace.TokenClass != TokenClass.Operator)
                return new Production(null, tokens);
            var remainder = tokens.ToList();
            Constituents.Add(left);
            while (remainder.Any() && Constituents.Count < 3) // An operation must have exactly three items: value op value
            {
                var currentToken = remainder.First().TokenClass;
                Production current = null;
                //check whether the right hand side of the operation is another operation
                if (Constituents.Count == 2) current = new OperationExpression().Parse(remainder);
                if (Constituents.Count != 2 || current?.Expression == null)
                    switch (currentToken)
                    {
                        case TokenClass.StartScope:
                            current = new ScopedExpression().Parse(remainder);
                            break;
                        case TokenClass.Whitespace:
                            current = new WhitespaceExpression().Parse(remainder);
                            break;
                        case TokenClass.ReplacementSymbol:
                            current = new ReplacementSymbolExpression().Parse(remainder);
                            break;
                        case TokenClass.Symbol:
                            current = new FunctionExpression().Parse(remainder);
                            if (current.Expression == null) current = new SymbolExpression().Parse(remainder);
                            break;
                        case TokenClass.Operator:
                            if (Constituents.Count == 2)
                            {
                                current = NumericExpression.TryParseNumeric(remainder);
                                break;
                            }
                            nonWhitespace = remainder.NextNonWhitespace(); // if there's nothing after the operator e.g. 1 +
                            if (nonWhitespace == null) return new Production(null, tokens);
                            current = new OperatorExpression().Parse(remainder);
                            break;
                        case TokenClass.Integer:
                        case TokenClass.Float:
                            current = NumericExpression.TryParseNumeric(remainder);
                            break;
                        case TokenClass.String:
                            current = new StringExpression().Parse(remainder);
                            break;
                        case TokenClass.Boolean:
                            current = new BooleanExpression().Parse(remainder);
                            break;
                        default:
                            return new Production(null, tokens);
                    }
                if (current == null || current.Expression == null) return new Production(null, tokens);
                remainder = current.RemainingTokens;
                if (!(current.Expression is WhitespaceExpression)) Constituents.Add(current.Expression);
            }
            if (Constituents.None(c => c is OperatorExpression)) return new Production(null, tokens);
            if (Constituents.Count != 3) return new Production(null, tokens);
            return new Production(this, remainder);
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values, FunctionSource functions)
        {
            EvaluationResult left = Constituents[0].Evaluate(numericPrecision, values, functions);
            OperatorExpression op = Constituents[1] as OperatorExpression;
            EvaluationResult right = Constituents[2].Evaluate(numericPrecision, values, functions);
            return Evaluate(left, op, right, numericPrecision);
        }

        private EvaluationResult Evaluate(EvaluationResult left, OperatorExpression op, EvaluationResult right, NumericPrecision numericPrecision)
        {
            Exception exception = null;
            try
            {
                if (left == null || op == null || right == null) return new EvaluationResult(EvaluationType.Null, null);
                if (left.Type == EvaluationType.Boolean && right.Type == EvaluationType.Boolean) return EvaluateBools(left.AsBoolean().Value, op, right.AsBoolean().Value);
                if (left.Type == EvaluationType.DateTime && right.Type == EvaluationType.DateTime) return EvaluateDates(left.AsDateTime().Value, op, right.AsDateTime().Value);
                if (left.Type == EvaluationType.String && right.Type == EvaluationType.String) return EvaluateStrings(left.AsString(), op, right.AsString());
                if (left.Type == EvaluationType.Int && right.Type == EvaluationType.Int) return EvaluateInts(left.AsInt().Value, op, right.AsInt().Value);
                var leftIsNumeric = left.Type == EvaluationType.Float || left.Type == EvaluationType.Int;
                var rightIsNumeric = right.Type == EvaluationType.Float || right.Type == EvaluationType.Int;
                if (leftIsNumeric && rightIsNumeric)
                {
                    var leftNumeric = left.AsRealNumber(numericPrecision);
                    var rightNumeric = right.AsRealNumber(numericPrecision);
                    return EvaluateFloats(leftNumeric, op, rightNumeric);
                }
            }
            catch (Exception e)
            {
                exception = e;
            }
            throw new EvaluationException(left, op, right, exception);
        }

        private EvaluationResult EvaluateInts(int left, OperatorExpression op, int right)
        {
            switch (op.Token.Lexeme)
            {
                case "+": return new EvaluationResult(EvaluationType.Int, left + right);
                case "-": return new EvaluationResult(EvaluationType.Int, left - right);
                case "*": return new EvaluationResult(EvaluationType.Int, left * right);
                case "/": return new EvaluationResult(EvaluationType.Int, left / right);
                case ">": return new EvaluationResult(EvaluationType.Boolean, left > right);
                case "<": return new EvaluationResult(EvaluationType.Boolean, left < right);
                case "=": return new EvaluationResult(EvaluationType.Boolean, left == right);
                case "!=": return new EvaluationResult(EvaluationType.Boolean, left != right);
                case ">=": return new EvaluationResult(EvaluationType.Boolean, left >= right);
                case "<=": return new EvaluationResult(EvaluationType.Boolean, left <= right);
                default: throw new Exception("Unexpected operator: " + op.Token.Lexeme);
            }
        }

        private EvaluationResult EvaluateFloats(RealNumber left, OperatorExpression op, RealNumber right)
        {
            switch (op.Token.Lexeme)
            {
                case "+": return new EvaluationResult(EvaluationType.Float, left + right);
                case "-": return new EvaluationResult(EvaluationType.Float, left - right);
                case "*": return new EvaluationResult(EvaluationType.Float, left * right);
                case "/": return new EvaluationResult(EvaluationType.Float, left / right);
                case ">": return new EvaluationResult(EvaluationType.Boolean, left > right);
                case "<": return new EvaluationResult(EvaluationType.Boolean, left < right);
                case "=": return new EvaluationResult(EvaluationType.Boolean, left == right);
                case "!=": return new EvaluationResult(EvaluationType.Boolean, left != right);
                case ">=": return new EvaluationResult(EvaluationType.Boolean, left >= right);
                case "<=": return new EvaluationResult(EvaluationType.Boolean, left <= right);
                default: throw new Exception("Unexpected operator: " + op.Token.Lexeme);
            }
        }

        private EvaluationResult EvaluateDates(DateTime left, OperatorExpression op, DateTime right)
        {
            switch (op.Token.Lexeme)
            {
                case "+": return new EvaluationResult(EvaluationType.Float, left + right.TimeOfDay);
                case "-": return new EvaluationResult(EvaluationType.Float, left - right);
                case "*": throw new NotImplementedException();
                case "/": throw new NotImplementedException();
                case ">": return new EvaluationResult(EvaluationType.Boolean, left > right);
                case "<": return new EvaluationResult(EvaluationType.Boolean, left < right);
                case "=": return new EvaluationResult(EvaluationType.Boolean, left == right);
                case "!=": return new EvaluationResult(EvaluationType.Boolean, left != right);
                case ">=": return new EvaluationResult(EvaluationType.Boolean, left >= right);
                case "<=": return new EvaluationResult(EvaluationType.Boolean, left <= right);
                default: throw new Exception("Unexpected operator: " + op.Token.Lexeme);
            }
        }

        private EvaluationResult EvaluateStrings(string left, OperatorExpression op, string right)
        {
            switch (op.Token.Lexeme)
            {
                case "+": return new EvaluationResult(EvaluationType.String, left + right);
                case "-": return new EvaluationResult(EvaluationType.String, left.Replace(right, ""));
                case "*": throw new NotImplementedException();
                case "/": throw new NotImplementedException();
                case ">": return new EvaluationResult(EvaluationType.Boolean, left.Length > right.Length);
                case "<": return new EvaluationResult(EvaluationType.Boolean, left.Length < right.Length);
                case "=": return new EvaluationResult(EvaluationType.Boolean, left == right);
                case "!=": return new EvaluationResult(EvaluationType.Boolean, left != right);
                case ">=": return new EvaluationResult(EvaluationType.Boolean, left.Length >= right.Length);
                case "<=": return new EvaluationResult(EvaluationType.Boolean, left.Length <= right.Length);
                default: throw new Exception("Unexpected operator: " + op.Token.Lexeme);
            }
        }

        private EvaluationResult EvaluateBools(bool left, OperatorExpression op, bool right)
        {
            switch (op.Token.Lexeme)
            {
                case "+": throw new NotImplementedException();
                case "-": throw new NotImplementedException();
                case "*": throw new NotImplementedException();
                case "/": throw new NotImplementedException();
                case ">": throw new NotImplementedException();
                case "<": throw new NotImplementedException();
                case "=": return new EvaluationResult(EvaluationType.Boolean, left == right);
                case "!=": return new EvaluationResult(EvaluationType.Boolean, left != right);
                case ">=": throw new NotImplementedException();
                case "<=": throw new NotImplementedException();
                default: throw new Exception("Unexpected operator: " + op.Token.Lexeme);
            }
        }
    }
}
