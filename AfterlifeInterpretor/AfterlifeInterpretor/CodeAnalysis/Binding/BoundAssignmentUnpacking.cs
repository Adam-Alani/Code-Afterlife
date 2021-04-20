using System;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundAssignmentUnpacking : BoundExpression
    {
        public BoundBinaryExpression Assignee { get; }
        public BoundExpression Assignment { get; }

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
        public override int Position { get; }
        public override Type Type => Assignment.Type;

        public BoundAssignmentUnpacking(BoundBinaryExpression assignee, BoundExpression assignment, int position)
        {
            Assignee = assignee;
            Position = position;
            Assignment = assignment;
        }
    }
}