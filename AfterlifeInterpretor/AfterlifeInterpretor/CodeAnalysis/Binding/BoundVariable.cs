using System;
using AfterlifeInterpretor.CodeAnalysis.Syntax;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundVariable : BoundExpression
    {
        public string Name { get;  }
        public override Type Type { get; protected set; }
        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;
    
        public BoundVariable(string name, Type type)
        {
            Name = name;
            Type = type;
        }
    }
}