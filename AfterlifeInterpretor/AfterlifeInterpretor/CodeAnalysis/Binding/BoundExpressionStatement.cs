namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundExpressionStatement : BoundStatement
    {
        public BoundExpression Expression { get; }
        public override BoundNodeKind Kind => BoundNodeKind.ExpressionStatement;
        public override int Position { get; }

        public BoundExpressionStatement(BoundExpression expression, int position)
        {
            Expression = expression;
            Position = position;
        }
    }
}