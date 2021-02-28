namespace AfterlifeInterpretor.CodeAnalysis.Syntax
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
        OParenToken,
        CParenToken,
        OStatementToken,
        CStatementToken,
        IdentifierToken,
        VarToken,
        BoolToken,
        IntToken,
        EndToken,
        
        // Operator Tokens
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        ModuloToken,
        
        AndToken,
        OrToken,
        NotToken,
        NEqToken,
        EqToken,
        GtEqToken,
        GtToken,
        LtEqToken,
        LtToken,
        
        AssignToken,
        
        
        // Expressions
        LiteralExpression,
        BinaryExpression,
        ParenthesisedExpression,
        UnaryExpression,
        AssignementExpression,
        VariableExpression,
        
        // Statements
        BlockStatement,
        ExpressionStatement,
        
        // Keywords
        TrueKeyword,
        FalseKeyword,
        
    }
}