namespace Expressive.Core.Language.Interpreter
{
    /// <summary>
    /// A function which can be invoked by the interpreter. 
    /// If the function is intended to be variadic, the supplied parameter will be of EvaluationType List, where the
    /// List will be of type EvaluationResult.
    /// If the function is intended to be parameterless, the supplied parameter will be of EvaluationType Null.
    /// <see cref="EvaluationResult"/>
    /// </summary>
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
