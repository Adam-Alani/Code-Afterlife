using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundBinary : BoundExpression
    {
        public BoundExpression Left { get;  }
        public BoundBinaryKind OperatorKind { get;  }
        public BoundExpression Right { get;  }
        
        public override Type Type => Left.Type;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        
        public BoundBinary(BoundExpression left, BoundBinaryKind kind, BoundExpression right)
        {
            Left = left;
            OperatorKind = kind;
            Right = right;
        }
    }
}