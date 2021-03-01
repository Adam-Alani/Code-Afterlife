namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundExpressionStatement : BoundStatement
    {
        public BoundExpression Expression { get; }
        public override BoundNodeKind Kind => BoundNodeKind.ExpressionStatement;

        public BoundExpressionStatement(BoundExpression expression)
        {
            Expression = expression;
        }
    }
}