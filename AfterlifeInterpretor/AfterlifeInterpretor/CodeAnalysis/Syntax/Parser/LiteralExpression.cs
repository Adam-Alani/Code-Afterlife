using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
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
        public object Value { get; }
        
        public LiteralExpression(SyntaxToken token)
        {
            Token = token;
            Value = token.Value;
        }

        public LiteralExpression(SyntaxToken token, object value)
        {
            Token = token;
            Value = value;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
        }
    }
}