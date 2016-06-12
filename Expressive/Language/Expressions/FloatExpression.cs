using Expressive.Core.Language.Interpreter;

namespace Expressive.Core.Language.Expressions
{
    public class FloatExpression : NumericExpression
    {
        public FloatExpression() 
            : base(TokenClass.Float)
        {
        }

        public override EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values, FunctionSource functions)
            => numericPrecision == NumericPrecision.Float ? 
            new EvaluationResult(EvaluationType.Float, float.Parse(ToString())) :
            new EvaluationResult(EvaluationType.Float, decimal.Parse(ToString()));
    }
}
