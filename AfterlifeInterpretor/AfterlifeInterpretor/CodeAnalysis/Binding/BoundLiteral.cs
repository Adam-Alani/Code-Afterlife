using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundLiteral : BoundExpression
    {
        public object Value { get; }

        public override Type Type => Value.GetType();
        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
        
        public BoundLiteral(object value)
        {
            Value = value;
        }
    }
}