using System;
using Expressive.Core.Language.Expressions;
using Expressive.Core.Language.Interpreter;

namespace Expressive.Core.Exceptions
{
    public class EvaluationException : Exception
    {
        public Expression Expression { get; set; }
        public EvaluationResult Left { get; set; }
        public OperatorExpression Op { get; set; }
        public EvaluationResult Right { get; set; }

        public EvaluationException(Expression expression, Exception innerException) 
            : base($"Expression '{expression}' could not be evaluated, see contained expression for details", innerException)
        {
            Expression = expression;
        }

        public EvaluationException(Expression expression)
            : this(expression, null)
        {
        }

        public EvaluationException(EvaluationResult left, OperatorExpression op, EvaluationResult right, Exception innerException)
            : base($"Expression '{left.AsString()} {op.ToString()} {right.AsString()}' could not be evaluated, see contained expression for details", innerException)
        {
            Left = left;
            Op = op;
            Right = right;
        }

        public EvaluationException(EvaluationResult left, OperatorExpression op, EvaluationResult right)
            : this(left, op, right, null)
        {
        }
    }
}
