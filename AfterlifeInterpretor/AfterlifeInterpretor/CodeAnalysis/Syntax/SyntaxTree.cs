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
        public Errors Errs { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken End { get;  }
        
        public SyntaxTree(Errors errors, ExpressionSyntax root, SyntaxToken end)
        {
            Errs = errors;
            Root = root;
            End = end;
        }
    }
}