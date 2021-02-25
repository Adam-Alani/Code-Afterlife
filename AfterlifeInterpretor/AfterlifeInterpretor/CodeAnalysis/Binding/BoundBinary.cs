using System;
using AfterlifeInterpretor.CodeAnalysis.Syntax;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundBinary : BoundExpression
    {
        public BoundExpression Left { get;  }
        public BoundBinaryOperator Operator { get;  }
        public BoundExpression Right { get;  }
        
        public override Type Type
        {
            get => Operator.ResultType;
            protected set => Type = Type;
        }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
        
        public BoundBinary(BoundExpression left, BoundBinaryOperator op, BoundExpression right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }
}