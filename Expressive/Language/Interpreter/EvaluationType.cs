namespace Expressive.Core.Language.Interpreter
{
    /// <summary>
    /// The actual type of an EvaluationResult.
    /// <see cref="EvaluationResult"/>
    /// </summary>
    public enum EvaluationType
    {
        Int,
        Float,
        String,
        /// <summary>
        /// Exists a result or input; there is no DateTime-literal syntax in Expressive
        /// </summary>
        DateTime, 
        Boolean,
        Enumerable,
        /// <summary>
        /// An unevaluated expression as a string. Differentiated from 'String' to avoid unintentionally evaluating raw strings.
        /// If a function or replacement symbol resolves to an EvaluationResult of type Expression, the expression will be evaluated
        /// by the interpreter. 
        /// Should never be returned by the interpreter.
        /// </summary>
        Expression,
        /// <summary>
        /// Exists a result or input; there is no null-literal syntax in Expressive
        /// </summary>
        Null
    }
}