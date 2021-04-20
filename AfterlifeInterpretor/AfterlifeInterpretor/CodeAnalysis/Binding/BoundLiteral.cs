using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundLiteral : BoundExpression
    {
        public object Value { get; }

        public override Type Type => Value.GetType();
        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
        public override int Position { get; }

        public BoundLiteral(object value, int position)
        {
            Value = value;
            Position = position;
        }
    }
}