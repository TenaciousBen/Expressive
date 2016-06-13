namespace Expressive.Core.Language.Interpreter
{
    public enum EvaluationType
    {
        Int,
        Float,
        String,
        // Provided for through functions - there is no date-literal syntax in Expressive
        DateTime, 
        Boolean,
        Enumerable,
        // An unevaluated expression as a string. Differentiated from 'String' to avoid unintentionally evaluating raw strings. 
        // Should never be returned by the interpreter.
        Expression,
        // Exists a result or input; there is no null-literal syntax in Expressive
        Null
    }
}