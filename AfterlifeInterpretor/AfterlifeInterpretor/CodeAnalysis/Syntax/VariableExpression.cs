using System;
using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax
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