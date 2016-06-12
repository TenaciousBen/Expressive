namespace Expressive.Core.Language.Interpreter
{
    public enum EvaluationType
    {
        Int,
        Float,
        String,
        DateTime, // provided for through functions - there is no date-literal syntax in Expressive
        Boolean,
        Enumerable,
        Null // exists a result or input - there is no null-literal syntax in Expressive
    }
}