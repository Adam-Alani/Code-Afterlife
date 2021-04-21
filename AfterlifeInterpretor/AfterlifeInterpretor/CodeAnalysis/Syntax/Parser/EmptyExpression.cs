using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public class EmptyExpression : ExpressionSyntax
    {
        public EmptyExpression(SyntaxToken token)
        {
            Token = token;
        }

        public override SyntaxKind Kind => SyntaxKind.EmptyExpression;
        public override SyntaxToken Token { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
        }
    }
}