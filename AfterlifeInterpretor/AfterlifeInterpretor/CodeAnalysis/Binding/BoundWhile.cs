namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundWhile : BoundStatement
    {
        public BoundStatement Then { get; }

        public BoundExpressionStatement Condition { get; }
        
        public override BoundNodeKind Kind => BoundNodeKind.WhileStatement;
        public override int Position { get; }

        public BoundWhile(BoundExpressionStatement condition, BoundStatement then, int position)
        {
            Condition = condition;
            Then = then;
            Position = position;
        }
    }
}