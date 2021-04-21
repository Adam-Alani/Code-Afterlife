using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundIfExpression : BoundExpression
    {
        public BoundExpression Else { get; }

        public BoundExpression Then { get; }

        public BoundExpression Condition { get; }
        
        public override BoundNodeKind Kind => BoundNodeKind.IfExpression;
        public override int Position { get; }

        public override Type Type => Then.Type;

        public BoundIfExpression(BoundExpression condition, BoundExpression then, BoundExpression elseClause, int position)
        {
            Condition = condition;
            Then = then;
            Else = elseClause;
            Position = position;
        }
    }
}