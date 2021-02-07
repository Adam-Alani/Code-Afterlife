using System.Collections.Generic;

namespace AfterlifeInterpretor
{
    /// <summary>
    /// Syntax token class
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    public class SyntaxToken : SyntaxNode
    {
        public override SyntaxKind Kind { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }
        
        public SyntaxToken(SyntaxKind kind, int position, string text, object value = null)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return this;
        }
    }
}