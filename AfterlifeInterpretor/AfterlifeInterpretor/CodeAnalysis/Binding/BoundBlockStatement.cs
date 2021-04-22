using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundBlockStatement : BoundStatement
    {
        public BoundStatement[] Statements { get; }
        public override BoundNodeKind Kind => BoundNodeKind.BlockStatement;
        public override int Position { get; }

        public override Type Type { get; }
        public override string TypeString { get; }


        public BoundBlockStatement(BoundStatement[] statements, int position, Type type = null, string typeString = null)
        {
            Statements = statements;
            Position = position;
            Type = type;
            TypeString = typeString ?? ((Type != null) ? Text.PrettyType(Type) : "()");
        }
    }
}