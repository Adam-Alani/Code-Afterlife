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
        public override string TypeString { get; }

        public Function F { get; }
        
        public int Depth { get;  }

        public BoundCallExpression(BoundExpression called, BoundExpression args, Type type, int position,
            Function f = null, int depth = 1, string typeString = null)
        {
            Called = called;
            Args = args;
            Position = position;
            Type = type;
            F = f;
            Depth = depth;
            TypeString = typeString ?? ((Type != null) ? Text.PrettyType(Type) : "()");
        }
    }
}