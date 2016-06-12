namespace Expressive.Core.Language.Expressions
{
    /// <summary>
    /// Represents an expression which can have no constituent expressions, such as a value type.
    /// </summary>
    public abstract class TerminatingExpression : Expression
    {
        public override string ToString()
        {
            return base.Token.Lexeme;
        }
    }
}
