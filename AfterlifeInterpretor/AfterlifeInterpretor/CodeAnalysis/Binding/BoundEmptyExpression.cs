using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundEmptyExpression : BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.EmptyExpression;
        public override int Position { get; }
        public override Type Type => typeof(object);
        public override string TypeString => "()";


        public BoundEmptyExpression(int position)
        {
            Position = position;
        }

        public override string ToString()
        {
            return "()";
        }
    }
}