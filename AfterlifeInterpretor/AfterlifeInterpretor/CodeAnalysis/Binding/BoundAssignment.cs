using System;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Binding
{
    internal sealed class BoundAssignment : BoundExpression
    {
        public BoundVariable Assignee { get; }
        public BoundExpression Assignment { get; }

        public override BoundNodeKind Kind => BoundNodeKind.AssignmentExpression;
        public override Type Type => Assignment.Type;

        public BoundAssignment(BoundVariable assignee, BoundExpression assignment, SyntaxKind op)
        {
            Assignee = assignee;
            Assignment = GetAssignment(assignment, op);
        }

        private BoundExpression GetAssignment(BoundExpression assignment, SyntaxKind op)
        {
            switch (op)
            {
                case SyntaxKind.PlusAssignToken:
                case SyntaxKind.MinusAssignToken:
                case SyntaxKind.StarAssignToken:
                case SyntaxKind.SlashAssignToken:
                case SyntaxKind.ModuloAssignToken:
                    return new BoundBinaryExpression(Assignee, BoundBinaryOperator.Bind(op, Assignee.Type, assignment.Type), assignment);
                default:
                    return assignment;
            }
        }
    }
}