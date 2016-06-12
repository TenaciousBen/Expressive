using Expressive.Core.Language.Interpreter;

namespace Expressive.Core.Language.Expressions
{
    public class IntegerExpression : NumericExpression
    {
        public IntegerExpression() 
            : base(TokenClass.Integer)
        {
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values, FunctionSource functions)
            => int.Parse(ToString());
    }
}
