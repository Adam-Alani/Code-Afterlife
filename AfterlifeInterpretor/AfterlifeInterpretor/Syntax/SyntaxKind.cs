namespace AfterlifeInterpretor
{
    /// <summary>
    /// Syntax Kind enum
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    public enum SyntaxKind
    {
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
        
        NumericExpression,
        BinaryExpression,
        ParenthesisedExpression
    }
}