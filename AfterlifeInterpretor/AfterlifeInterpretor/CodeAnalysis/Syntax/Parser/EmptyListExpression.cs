using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public class EmptyListExpression : ExpressionSyntax
    {
        public EmptyListExpression()
        {
        }

        public override SyntaxKind Kind => SyntaxKind.EmptyListExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return null;
        }
    }
}