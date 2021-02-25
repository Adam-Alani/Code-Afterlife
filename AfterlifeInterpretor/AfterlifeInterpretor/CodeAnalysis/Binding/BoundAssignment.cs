using System;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundAssignment : BoundExpression
    {
        public BoundVariable Assignee { get; }
        public BoundExpression Assignment { get; }

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
        public override Type Type => Assignment.Type;

        public BoundAssignment(BoundVariable assignee, BoundExpression assignment)
        {
            Assignee = assignee;
            Assignment = assignment;
        }

        
    }
}