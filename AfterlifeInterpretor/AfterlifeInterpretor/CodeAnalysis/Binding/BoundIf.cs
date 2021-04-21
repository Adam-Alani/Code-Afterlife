using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundIf : BoundStatement
    {
        public BoundStatement Else { get; }

        public BoundStatement Then { get; }

        public BoundExpressionStatement Condition { get; }
        
        public override BoundNodeKind Kind => BoundNodeKind.IfStatement;
        public override int Position { get; }

        public override Type Type => Then.Type;

        public BoundIf(BoundExpressionStatement condition, BoundStatement then, BoundStatement elseClause, int position)
        {
            Condition = condition;
            Then = then;
            Else = elseClause;
            Position = position;
        }
    }
}