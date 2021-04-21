using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public sealed class ExpressionStatement : StatementSyntax
    {
        public ExpressionSyntax Expression { get; }
        public override SyntaxKind Kind => SyntaxKind.ExpressionStatement;
        public override SyntaxToken Token { get; }

        public ExpressionStatement(ExpressionSyntax expression)
        {
            Expression = expression;
            Token = Expression.Token;
        }
        
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Expression.GetChildren();
        }
    }
}