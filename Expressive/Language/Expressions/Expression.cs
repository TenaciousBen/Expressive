using System.Collections.Generic;
using Apirion.Expressive.Core.Extensions;
using Apirion.Expressive.Core.Language.Interpreter;

namespace Apirion.Expressive.Core.Language.Expressions
{
    public abstract class Expression
    {
        public Token Token { get; set; }
        /// <summary>
        /// Sub expressions which must be evaluated in order to evaluate to the parent expression, e.g. the
        /// parameters of a function or the operands of an operation.
        /// </summary>
        public List<Expression> Constituents { get; set; }
        public override string ToString() { return Constituents.StringConcat(); }

        protected Expression()
        {
            Constituents = new List<Expression>();
        }

        public abstract Production Parse(List<Token> tokens);

        public abstract EvaluationResult Evaluate(NumericPrecision numericPrecision, ValueSource values, FunctionSource functions);
    }
}
