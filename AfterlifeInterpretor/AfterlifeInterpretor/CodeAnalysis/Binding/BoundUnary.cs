using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundUnary : BoundExpression
    {
        public BoundUnaryOperator Operator { get;  }
        public BoundExpression Operand { get;  }
        
        public override Type Type
        {
            get => Operator.ResultType;
            protected set => Type = Type;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        
        public BoundUnary(BoundUnaryOperator op, BoundExpression operand)
        {
            Operator = op;
            Operand = operand;
        }
    }
}