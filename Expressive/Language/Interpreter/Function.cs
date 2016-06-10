using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apirion.Expressive.Core.Language.Interpreter
{
    public delegate EvaluationResult ExternalFunction(EvaluationResult result);

    public class Function
    {
        private ExternalFunction UnderlyingFunction { get; set; }

        public Function(ExternalFunction underlyingFunction)
        {
            UnderlyingFunction = underlyingFunction;
        }

        public EvaluationResult Invoke(EvaluationResult parameters) => UnderlyingFunction(parameters);
    }
}
