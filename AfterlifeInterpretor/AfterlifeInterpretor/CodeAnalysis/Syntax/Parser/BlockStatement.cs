using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public sealed class BlockStatement : StatementSyntax
    {
        public override SyntaxToken Token { get; }
        public StatementSyntax[] Statements { get; }
        public SyntaxToken CloseToken { get; }
        public override SyntaxKind Kind => SyntaxKind.BlockStatement;

        public BlockStatement(SyntaxToken openToken, StatementSyntax[] statements, SyntaxToken closeToken)
        {
            Token = openToken;
            Statements = statements;
            CloseToken = closeToken;
        }
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
            foreach (StatementSyntax s in Statements)
            {
                yield return s;
            }
            yield return CloseToken;
        }
    }
}