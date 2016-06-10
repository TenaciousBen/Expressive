namespace Apirion.Expressive.Core.Language
{
    public enum TokenClass
    {
        StartScope,
        EndScope,
        Separator,
        Whitespace,
        ReplacementSymbol,
        Symbol,
        Operator,
        Integer,
        Float,
        String,
        Boolean,
        Error
    }
}