using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public sealed class IfExpression : ExpressionSyntax
    {
        public override SyntaxToken Token { get; }
        public ExpressionStatement Condition { get; }
        public ExpressionSyntax Then { get; }
        public ExpressionSyntax Else { get; }
        
        public override SyntaxKind Kind => SyntaxKind.IfExpression;

        public IfExpression(SyntaxToken token, ExpressionStatement condition, ExpressionSyntax then, ExpressionSyntax elseClause)
        {
            Token = token;
            Condition = condition;
            Then = then;
            Else = elseClause;
        }

        

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
            yield return Condition;
            yield return Then;
            yield return Else;
        }
    }
}