using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax
{
    public sealed class IfStatement : StatementSyntax
    {
        public SyntaxToken Token { get; }
        public ExpressionStatement Condition { get; }
        public StatementSyntax Then { get; }
        public ElseClause Else { get; }
        
        public override SyntaxKind Kind => SyntaxKind.IfStatement;

        public IfStatement(SyntaxToken token, ExpressionStatement condition, StatementSyntax then)
        {
            Token = token;
            Condition = condition;
            Then = then;
            Else = null;
        }
        
        public IfStatement(SyntaxToken token, ExpressionStatement condition, StatementSyntax then, ElseClause elseClause)
        {
            Token = token;
            Condition = condition;
            Then = then;
            Else = elseClause;
        }

        

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
            yield return Condition;
            yield return Then;
            yield return Else;
        }
    }
}