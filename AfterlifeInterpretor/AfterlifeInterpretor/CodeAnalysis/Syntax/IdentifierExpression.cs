using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax
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