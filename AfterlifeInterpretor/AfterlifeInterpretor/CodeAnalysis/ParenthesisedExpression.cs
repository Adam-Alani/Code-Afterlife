using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis
{
    /// <summary>
    /// ParenthesisedExpression class
    /// Any expression in-between parentheses (e.g: (1 + 2 - 3))
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    public sealed class ParenthesisedExpression : ExpressionSyntax
    {
        public SyntaxToken OpenToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken CloseToken { get; }
        public override SyntaxKind Kind => SyntaxKind.ParenthesisedExpression;

        public ParenthesisedExpression(SyntaxToken openToken, ExpressionSyntax expression, SyntaxToken closeToken)
        {
            OpenToken = openToken;
            Expression = expression;
            CloseToken = closeToken;
        }
        
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenToken;
            yield return Expression;
            yield return CloseToken;
        }
    }
}