using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal abstract class BoundNode
    {
        public abstract BoundNodeKind Kind { get;  }
        
        public abstract int Position { get; }
        
        public abstract Type Type { get; }
        
        public abstract string TypeString { get; }
    }
}