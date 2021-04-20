using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundUnary : BoundExpression
    {
        public BoundUnaryOperator Operator { get;  }
        public BoundExpression Operand { get;  }
        
        public override Type Type => Operator.ResultType;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        public override int Position { get; }

        public BoundUnary(BoundUnaryOperator op, BoundExpression operand, int position)
        {
            Operator = op;
            Operand = operand;
            Position = position;
        }
    }
}