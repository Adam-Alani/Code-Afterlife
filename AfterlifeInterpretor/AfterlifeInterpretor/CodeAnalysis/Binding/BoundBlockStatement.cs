namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundBlockStatement : BoundStatement
    {
        public BoundStatement[] Statements { get; }
        public override BoundNodeKind Kind => BoundNodeKind.BlockStatement;

        public BoundBlockStatement(BoundStatement[] statements)
        {
            Statements = statements;
        }
    }
}