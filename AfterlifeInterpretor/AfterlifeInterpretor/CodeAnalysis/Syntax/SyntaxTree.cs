using System.Collections.Generic;

namespace AfterlifeInterpretor.CodeAnalysis.Syntax
{
    /// <summary>
    /// SyntaxTree class
    /// A tree parsed by a parser
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    public sealed class SyntaxTree
    {
        public List<string> Errors { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken End { get;  }
        
        public SyntaxTree(List<string> errors, ExpressionSyntax root, SyntaxToken end)
        {
            Errors = errors;
            Root = root;
            End = end;
        }
    }
}