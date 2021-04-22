using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Parser
{
    public sealed class ForStatement : StatementSyntax
    {
        public override SyntaxToken Token { get; }
        
        public ExpressionStatement Initialisation { get; }
        public ExpressionStatement Condition { get; }
        
        public ExpressionStatement Incrementation { get; }
        public StatementSyntax Then { get; }
        public override SyntaxKind Kind => SyntaxKind.ForStatement;
        public ForStatement(SyntaxToken token, ExpressionStatement initialisation, ExpressionStatement condition, ExpressionStatement incrementation, StatementSyntax then)
        {
            Token = token;
            Initialisation = initialisation;
            Condition = condition;
            Incrementation = incrementation;
            Then = then;
        }
        
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Token;
            yield return Initialisation;
            yield return Condition;
            yield return Incrementation;
            yield return Then;
        }
    }
}