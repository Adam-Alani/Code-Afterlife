using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundBinary : BoundExpression
    {
        public BoundExpression Left { get;  }
        public BoundBinaryOperator Operator { get;  }
        public BoundExpression Right { get;  }
        
        public override Type Type => Operator?.ResultType;
        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpression;
        public override int Position { get; }
        public override string TypeString => Text.PrettyType(Type);


        public BoundBinary(BoundExpression left, BoundBinaryOperator op, BoundExpression right, int position)
        {
            Left = left;
            Operator = op;
            Right = right;
            Position = position;
        }
    }
}