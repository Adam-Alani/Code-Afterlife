using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax
{
    public sealed class BlockStatement : StatementSyntax
    {
        public SyntaxToken OpenToken { get; }
        public StatementSyntax[] Statements { get; }
        public SyntaxToken CloseToken { get; }
        public override SyntaxKind Kind => SyntaxKind.BlockStatement;

        public BlockStatement(SyntaxToken openToken, StatementSyntax[] statements, SyntaxToken closeToken)
        {
            OpenToken = openToken;
            Statements = statements;
            CloseToken = closeToken;
        }
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenToken;
            foreach (StatementSyntax s in Statements)
            {
                yield return s;
            }
            yield return CloseToken;
        }
    }
}