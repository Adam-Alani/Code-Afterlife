using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public sealed class UnaryExpression : ExpressionSyntax
    {
        
        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
        public ExpressionSyntax Operand { get; }
        public override SyntaxToken Token { get; }
        
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