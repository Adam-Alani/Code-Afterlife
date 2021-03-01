namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundWhile : BoundStatement
    {
        public BoundStatement Then { get; }

        public BoundExpressionStatement Condition { get; }
        
        public override BoundNodeKind Kind => BoundNodeKind.WhileStatement;
        
        public BoundWhile(BoundExpressionStatement condition, BoundStatement then)
        {
            Condition = condition;
            Then = then;
        }
    }
}