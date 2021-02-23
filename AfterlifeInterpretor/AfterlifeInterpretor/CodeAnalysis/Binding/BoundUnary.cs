using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundUnary : BoundExpression
    {
        public BoundUnaryKind OperatorKind { get;  }
        public BoundExpression Operand { get;  }
        
        public override Type Type => Operand.Type;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        
        public BoundUnary(BoundUnaryKind kind, BoundExpression operand)
        {
            OperatorKind = kind;
            Operand = operand;
        }
    }
}