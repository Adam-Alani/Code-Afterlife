namespace AfterlifeInterpretor.CodeAnalysis.Syntax
{
    /// <summary>
    /// SyntaxFact class
    /// Static class that can be used to get information on how data is supposed to be parsed (i.e.: precedence etc.)
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    internal static class SyntaxFacts
    {
        public static int GetUnaryPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.NotToken:
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;
                default:
                    return 0;
            }
        }
        
        public static int GetBinaryPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.AndToken:
                case SyntaxKind.OrToken:
                    return 1;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 2;
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                case SyntaxKind.ModuloToken:
                    return 3;
                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeywordKind(string text)
        {
            return text switch
            {
                "true" => SyntaxKind.TrueKeyword,
                "false" => SyntaxKind.FalseKeyword,
                "and" => SyntaxKind.AndToken,
                "or" => SyntaxKind.OrToken,
                "not" => SyntaxKind.NotToken,
                _ => SyntaxKind.IdentifierToken
            };
        }
    }
}