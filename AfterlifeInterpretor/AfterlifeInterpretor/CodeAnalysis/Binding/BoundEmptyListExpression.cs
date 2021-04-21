using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundEmptyListExpression : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.EmptyListExpression;
        public override int Position { get; }
        public override Type Type => typeof(List);

        public BoundEmptyListExpression(int position)
        {
            Position = position;
        }
    }
}