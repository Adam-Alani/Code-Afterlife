using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public sealed class BinaryExpression : ExpressionSyntax
    {
        
        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
        public ExpressionSyntax Left { get; }
        public ExpressionSyntax Right { get; }
        public SyntaxToken Token { get; }
        
        public BinaryExpression(ExpressionSyntax l, SyntaxToken token, ExpressionSyntax r)
        {
            Left = l;
            Right = r;
            Token = token;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return Token;
            yield return Right;
        }
    }
}