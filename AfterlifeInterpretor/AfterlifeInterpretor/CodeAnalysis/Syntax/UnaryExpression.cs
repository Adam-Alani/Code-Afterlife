using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax
{
    public sealed class UnaryExpression : ExpressionSyntax
    {
        
        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
        public ExpressionSyntax Operand { get; }
        public SyntaxToken Token { get; }
        
        public UnaryExpression(SyntaxToken token, ExpressionSyntax operand)
        {
            Token = token;
            Operand = operand;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
            yield return Operand;
        }
    }
}