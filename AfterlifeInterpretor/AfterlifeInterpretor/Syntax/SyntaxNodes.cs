using System.Collections.Generic;

namespace AfterlifeInterpretor
{
    /// <summary>
    /// Syntax Node classes
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>

    public abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }

    public abstract class ExpressionSyntax : SyntaxNode
    {
    }
    
    public sealed class NumericExpression : ExpressionSyntax
    {
        public override SyntaxKind Kind => SyntaxKind.NumericExpression;
        public SyntaxToken NumericToken { get; }

        public NumericExpression(SyntaxToken token)
        {
            NumericToken = token;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return NumericToken;
        }
    }

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

    public sealed class SyntaxTree
    {
        public List<string> Errors { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken End { get;  }
        
        public SyntaxTree(List<string> errors, ExpressionSyntax root, SyntaxToken end)
        {
            Errors = errors;
            Root = root;
            End = end;
        }
    }
}