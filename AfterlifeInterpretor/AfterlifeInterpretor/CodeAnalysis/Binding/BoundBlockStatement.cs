namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundBlockStatement : BoundStatement
    {
        public BoundStatement[] Statements { get; }
        public override BoundNodeKind Kind => BoundNodeKind.BlockStatement;
        public override int Position { get; }

        public BoundBlockStatement(BoundStatement[] statements, int position)
        {
            Statements = statements;
            Position = position;
        }
    }
}