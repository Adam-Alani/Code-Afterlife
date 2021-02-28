namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        // Expressions
        UnaryExpression,
        LiteralExpression,
        VariableExpression,
        AssignmentExpression,

        // Statements
        BlockStatement,
        ExpressionStatement
    }
}