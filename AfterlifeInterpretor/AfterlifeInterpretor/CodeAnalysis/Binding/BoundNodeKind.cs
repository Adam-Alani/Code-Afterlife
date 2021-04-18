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

        // Statements
        BlockStatement,
        ExpressionStatement,
        IfStatement,
        WhileStatement,
        ForStatement,
    }
}