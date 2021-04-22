namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        // Expressions
        UnaryExpression,
        LiteralExpression,
        VariableExpression,
        AssignmentExpression,
        BinaryExpression,
        EmptyExpression,
        EmptyListExpression,
        CallExpression,
        IfExpression,

        // Statements
        BlockStatement,
        ExpressionStatement,
        IfStatement,
        WhileStatement,
        ForStatement,
        FunctionDeclaration,
        ReturnStatement,
    }
}