using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundEmptyExpression : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.EmptyExpression;
        public override Type Type => typeof(object);
    }
}