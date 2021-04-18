namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundFor : BoundStatement
    {
        public BoundStatement Then { get; }

        
        public BoundExpressionStatement Initialisation { get; }
        public BoundExpressionStatement Condition { get; }
        public BoundExpressionStatement Incrementation { get; }
        
        public override BoundNodeKind Kind => BoundNodeKind.ForStatement;
        
        public BoundFor(BoundExpressionStatement initialisation, BoundExpressionStatement condition, BoundExpressionStatement incrementation, BoundStatement then)
        {
            Initialisation = initialisation;
            Condition = condition;
            Incrementation = incrementation;
            Then = then;
        }
    }
}