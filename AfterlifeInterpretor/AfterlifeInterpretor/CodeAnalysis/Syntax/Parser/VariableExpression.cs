using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public sealed class VariableExpression : ExpressionSyntax
    {
        public IdentifierExpression Name { get; }
        public SyntaxToken Token { get; }
        public override SyntaxKind Kind => SyntaxKind.VariableExpression;

        public VariableExpression(SyntaxToken token, IdentifierExpression name)
        {
            Name = name;
            Token = token;
        }

        
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
            yield return Name;
        }
    }
}