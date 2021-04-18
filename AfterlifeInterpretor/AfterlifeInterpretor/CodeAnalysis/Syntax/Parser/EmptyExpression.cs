using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public class EmptyExpression : ExpressionSyntax
    {
        public EmptyExpression()
        {
        }

        public override SyntaxKind Kind => SyntaxKind.EmptyExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return null;
        }
    }
}