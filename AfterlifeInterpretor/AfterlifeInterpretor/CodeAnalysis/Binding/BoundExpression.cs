using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal abstract class BoundExpression : BoundNode
    {
        public abstract Type Type { get;  }
    }
}