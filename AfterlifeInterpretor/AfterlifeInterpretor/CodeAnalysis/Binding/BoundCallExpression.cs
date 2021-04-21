using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundCallExpression: BoundExpression
    {
        public override BoundNodeKind Kind => BoundNodeKind.CallExpression;
        public override int Position { get; }
        public BoundExpression Called { get; }
        public BoundExpression Args { get; }
        public override Type Type { get; }

        public BoundCallExpression(BoundExpression called, BoundExpression args, Type type, int position)
        {
            Called = called;
            Args = args;
            Position = position;
            Type = type;
        }
    }
}