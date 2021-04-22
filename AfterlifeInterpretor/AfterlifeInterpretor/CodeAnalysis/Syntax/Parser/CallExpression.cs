using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public sealed class CallExpression : ExpressionSyntax
    {
        public override SyntaxKind Kind => SyntaxKind.CallExpression;
        public override SyntaxToken Token { get; }

        public ExpressionSyntax Called { get; }
        
        public ExpressionSyntax Args { get; }

        public CallExpression(ExpressionSyntax called, ExpressionSyntax args)
        {
            Called = called;
            Token = called.Token;
            Args = args;
        }
        
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Called;
            yield return Args;
        }
    }
}