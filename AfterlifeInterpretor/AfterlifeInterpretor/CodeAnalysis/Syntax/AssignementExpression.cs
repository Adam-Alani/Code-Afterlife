using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax
{
    public sealed class AssignementExpression : ExpressionSyntax
    {
        
        public override SyntaxKind Kind => SyntaxKind.AssignementExpression;
        public ExpressionSyntax Assignee { get; }
        public ExpressionSyntax Assignment { get; }
        public SyntaxToken Token { get; }
        
        public AssignementExpression(ExpressionSyntax assignee, SyntaxToken token, ExpressionSyntax assignment)
        {
            Assignee = assignee;
            Assignment = assignment;
            Token = token;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Assignee;
            yield return Token;
            yield return Assignment;
        }
    }
}