using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundFor : BoundStatement
    {
        public BoundStatement Then { get; }

        
        public BoundExpressionStatement Initialisation { get; }
        public BoundExpressionStatement Condition { get; }
        public BoundExpressionStatement Incrementation { get; }
        
        public override BoundNodeKind Kind => BoundNodeKind.ForStatement;
        public override int Position { get; }
        public override Type Type => Then.Type;

        public BoundFor(BoundExpressionStatement initialisation, BoundExpressionStatement condition, BoundExpressionStatement incrementation, BoundStatement then, int position)
        {
            Initialisation = initialisation;
            Condition = condition;
            Incrementation = incrementation;
            Then = then;
            Position = position;
        }

        
    }
}