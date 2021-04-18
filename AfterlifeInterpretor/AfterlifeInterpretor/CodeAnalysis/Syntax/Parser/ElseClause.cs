using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public class ElseClause : SyntaxNode
    {
        public SyntaxToken Token { get; }
        public StatementSyntax Then { get; }

        public override SyntaxKind Kind => SyntaxKind.ElseClause;

        public ElseClause(SyntaxToken token, StatementSyntax then)
        {
            Token = token;
            Then = then;
        }

        
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
            yield return Then;
        }
    }
}