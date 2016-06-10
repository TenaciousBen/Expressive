using System.Collections.Generic;
using System.Linq;
using Apirion.Expressive.Core.Language.Interpreter;
using Apirion.Expressive.Core.Language.Types;

namespace Apirion.Expressive.Core.Language.Expressions
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
