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
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 3;
                default:
                    return 0;
            }
        }
        
        public static int GetBinaryPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 1;
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                case SyntaxKind.ModuloToken:
                    return 2;
                default:
                    return 0;
            }
        }
    }
}