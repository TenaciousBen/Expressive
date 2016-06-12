namespace Expressive.Core.Language.Interpreter
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
