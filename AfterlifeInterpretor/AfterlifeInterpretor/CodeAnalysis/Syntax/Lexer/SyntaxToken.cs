using System.Collections.Generic;
using AfterlifeInterpretor.CodeAnalysis.Syntax.Parser;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer
{
    /// <summary>
    /// Syntax token class
    /// Any kind of token tokenised by the lexer
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    public sealed class SyntaxToken : SyntaxNode
    {
        public override SyntaxKind Kind { get; }
        public override SyntaxToken Token { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }
        
        public SyntaxToken(SyntaxKind kind, int position, string text, object value = null)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
            Token = this;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return this;
        }
    }
}