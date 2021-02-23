namespace AfterlifeInterpretor.CodeAnalysis
{
    /// <summary>
    /// Syntax Kind enum
    /// Enumeration of all kinds of syntax tokens and expressions used by the tokeniser and the parser
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    public enum SyntaxKind
    {
        // Tokens
        ErrorToken,
        NumericToken,
        SpaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        ModuloToken,
        OParenToken,
        CParenToken,
        EndToken,
        
        // Expressions
        NumericExpression,
        BinaryExpression,
        ParenthesisedExpression
    }
}