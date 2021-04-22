using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    internal class EmptyListExpression : ExpressionSyntax
    {
        public EmptyListExpression(SyntaxToken token)
        {
            Token = token;
        }

        public override SyntaxKind Kind => SyntaxKind.EmptyListExpression;
        public override SyntaxToken Token { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
        }
    }
}