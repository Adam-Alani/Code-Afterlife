using System;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Parser;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundReturn : BoundStatement
    {
        public override BoundNodeKind Kind => BoundNodeKind.ReturnStatement;
        public override int Position { get; }

        public BoundExpression Expression;
        public override Type Type => Expression.Type;

        public BoundReturn(BoundExpression expression, int position)
        {
            Expression = expression;
            Position = position;
        }
    }
}