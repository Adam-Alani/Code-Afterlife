using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public sealed class WhileStatement : StatementSyntax
    {
        public SyntaxToken Token { get; }
        public ExpressionStatement Condition { get; }
        public StatementSyntax Then { get; }
        public override SyntaxKind Kind => SyntaxKind.WhileStatement;
        public WhileStatement(SyntaxToken token, ExpressionStatement condition, StatementSyntax then)
        {
            Token = token;
            Condition = condition;
            Then = then;
        }
        
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
            yield return Condition;
            yield return Then;
        }
    }
}