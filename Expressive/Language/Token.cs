namespace Expressive.Core.Language
{
    public class Token
    {
        public TokenClass TokenClass { get; set; }
        public string Lexeme { get; set; }

        public Token(TokenClass tokenClass, string lexeme)
        {
            TokenClass = tokenClass;
            Lexeme = lexeme;
        }

        public override string ToString()
        {
            return $"{TokenClass}: {Lexeme}";
        }
    }
}