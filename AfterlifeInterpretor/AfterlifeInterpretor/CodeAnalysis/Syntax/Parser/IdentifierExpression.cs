using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public sealed class IdentifierExpression : ExpressionSyntax
    {
        public SyntaxToken Token { get; }
        public override SyntaxKind Kind => SyntaxKind.IdentifierToken;

        public IdentifierExpression(SyntaxToken identifier)
        {
            Token = identifier;
        }

        
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
        }
    }
}