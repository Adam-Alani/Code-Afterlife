using System;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundAssignmentUnpacking : BoundExpression
    {
        public BoundBinary Assignee { get; }
        public BoundExpression Assignment { get; }

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
        public override int Position { get; }
        public override Type Type => Assignment?.Type;
        public override string TypeString => Assignment?.TypeString;


        public BoundAssignmentUnpacking(BoundBinary assignee, BoundExpression assignment, int position)
        {
            Assignee = assignee;
            Position = position;
            Assignment = assignment;
        }
    }
}