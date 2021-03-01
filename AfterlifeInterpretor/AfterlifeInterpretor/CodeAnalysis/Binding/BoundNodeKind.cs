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

        // Statements
        BlockStatement,
        ExpressionStatement,
        IfStatement,
        WhileStatement,
    }
}