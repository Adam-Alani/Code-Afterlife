namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer
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
        WordToken,
        SpaceToken,
        OParenToken,
        CParenToken,
        OBlockToken,
        CBlockToken,
        EndStatementToken,
        IdentifierToken,
        VarToken,
        BoolToken,
        IntToken,
        StringToken,
        FloatToken,
        ListToken,
        EndToken,
        PrintToken,
        
        // Operator Tokens
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        DoubleSlashToken,
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
        PlusAssignToken,
        MinusAssignToken,
        StarAssignToken,
        SlashAssignToken,
        ModuloAssignToken,
        
        SizeToken,
        HeadToken,
        TailToken,
        CommaToken,
        DotToken,

        // Keywords
        TrueKeyword,
        FalseKeyword,
        IfKeyword,
        ElseKeyword,
        WhileKeyword,
        ForKeyword,

        // Expressions
        LiteralExpression,
        BinaryExpression,
        ParenthesisedExpression,
        UnaryExpression,
        AssignementExpression,
        VariableExpression,
        EmptyExpression,
        EmptyListExpression,
        
        // Statements
        BlockStatement,
        ExpressionStatement,
        IfStatement,
        ElseClause,
        WhileStatement,
        ForStatement,
    }
}