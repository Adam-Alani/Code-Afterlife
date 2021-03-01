namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundIf : BoundStatement
    {
        public BoundStatement Else { get; }

        public BoundStatement Then { get; }

        public BoundExpressionStatement Condition { get; }
        
        public override BoundNodeKind Kind => BoundNodeKind.IfStatement;
        
        public BoundIf(BoundExpressionStatement condition, BoundStatement then, BoundStatement elseClause)
        {
            Condition = condition;
            Then = then;
            Else = elseClause;
        }
    }
}