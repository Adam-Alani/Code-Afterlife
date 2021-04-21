using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public sealed class FunctionDeclaration : ExpressionSyntax
    {
        public VariableExpression Variable { get;  }
        public ExpressionSyntax Args { get; }
        public StatementSyntax Body { get; }
        public override SyntaxKind Kind => SyntaxKind.FunctionDeclaration;
        public override SyntaxToken Token { get; }

        public FunctionDeclaration(SyntaxToken token, IdentifierExpression name, ExpressionSyntax args, StatementSyntax body)
        {
            Token = token;
            Variable = new VariableExpression(token, name);
            Args = args;
            Body = body;
        }
        
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}