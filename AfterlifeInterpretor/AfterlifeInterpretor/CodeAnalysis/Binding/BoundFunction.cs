using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundFunction : BoundExpression
    {
        public BoundVariable Assignee { get; }
        public BoundExpression Args { get; }
        public BoundStatement Body { get; }
        public override BoundNodeKind Kind => BoundNodeKind.FunctionDeclaration;
        
        public override int Position { get; }

        public override Type Type => Body?.Type;

        public BoundFunction(BoundVariable assignee, BoundExpression args, BoundStatement body, int position)
        {
            Assignee = assignee;
            Args = args;
            Body = body;
            Position = position;
        }

        public override string ToString()
        {
            return new Function(Args, Body, Type, null).ToString();
        }
    }
}