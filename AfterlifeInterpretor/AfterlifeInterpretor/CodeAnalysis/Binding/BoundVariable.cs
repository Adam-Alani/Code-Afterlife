using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundVariable : BoundExpression
    {
        public string Name { get;  }
        public override Type Type { get; }
        public override BoundNodeKind Kind => BoundNodeKind.VariableExpression;
        public override int Position { get; }

        public BoundVariable(string name, Type type, int position)
        {
            Name = name;
            Type = type;
            Position = position;
        }

        
    }
}