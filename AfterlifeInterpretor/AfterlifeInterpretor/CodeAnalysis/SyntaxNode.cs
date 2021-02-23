using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis
{
    /// <summary>
    /// SyntaxNode class
    /// Abstract class representing any kind of node in the syntax tree
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    public abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }

    
    /// <summary>
    /// ExpressionSyntax class
    /// Abstract type of node used for expressions
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    public abstract class ExpressionSyntax : SyntaxNode
    {
    }
}