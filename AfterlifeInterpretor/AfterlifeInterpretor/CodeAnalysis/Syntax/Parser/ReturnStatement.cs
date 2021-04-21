using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public sealed class ReturnStatement : StatementSyntax
    {
        public override SyntaxKind Kind => SyntaxKind.ReturnStatement;
        public override SyntaxToken Token { get; }
        public ExpressionSyntax Expression { get; }

        public ReturnStatement(SyntaxToken token, ExpressionSyntax expression)
        {
            Token = token;
            Expression = expression;
        }
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
            yield return Expression;
        }
    }
}