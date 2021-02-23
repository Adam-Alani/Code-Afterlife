using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis
{
    /// <summary>
    /// NumerixExpression class
    /// Nodes containing a literal expression (e.g.: 3, true, "hello")
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    public sealed class LiteralExpression : ExpressionSyntax
    {
        public override SyntaxKind Kind => SyntaxKind.LiteralExpression;
        public SyntaxToken Token { get; }

        public LiteralExpression(SyntaxToken token)
        {
            Token = token;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
        }
    }
}