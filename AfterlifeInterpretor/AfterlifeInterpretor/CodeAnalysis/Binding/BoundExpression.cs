using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal abstract class BoundExpression : BoundNode
    {
        public abstract Type Type { get; protected set; }

        public void SetType(Type type)
        {
            Type = type;
        }
    }
}