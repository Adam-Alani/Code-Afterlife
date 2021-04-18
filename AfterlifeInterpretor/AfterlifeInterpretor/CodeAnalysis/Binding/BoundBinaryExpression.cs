using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryExpression : BoundExpression
    {
        public BoundExpression Left { get;  }
        public BoundBinaryOperator Operator { get;  }
        public BoundExpression Right { get;  }
        
        public override Type Type => Operator.ResultType;
        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
        
        public BoundBinaryExpression(BoundExpression left, BoundBinaryOperator op, BoundExpression right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }
}