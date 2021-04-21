using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundExpressionStatement : BoundStatement
    {
        public BoundExpression Expression { get; }
        public override BoundNodeKind Kind => BoundNodeKind.ExpressionStatement;
        public override int Position { get; }

        public override Type Type => Expression.Type;

        public BoundExpressionStatement(BoundExpression expression, int position)
        {
            Expression = expression;
            Position = position;
        }

        
    }
}