using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public sealed class AssignementExpression : ExpressionSyntax
    {
        
        public override SyntaxKind Kind => SyntaxKind.AssignementExpression;
        public ExpressionSyntax Assignee { get; }
        public ExpressionSyntax Assignment { get; }
        public override SyntaxToken Token { get; }
        
        public AssignementExpression(ExpressionSyntax assignee, SyntaxToken token, ExpressionSyntax assignment)
        {
            Assignee = assignee;
            Token = token;
            Assignment = assignment;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Assignee;
            yield return Token;
            yield return Assignment;
        }
    }
}