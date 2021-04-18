namespace AfterlifeInterpretor.CodeAnalysis.Syntax.Lexer
{
    /// <summary>
    /// SyntaxFact class
    /// Static class that can be used to get information on how data is supposed to be parsed (i.e.: precedence etc.)
    /// Author: Raphaël "Sheinxy" Montes
    /// </summary>
    internal static class SyntaxFacts
    {
        public static int GetUnaryPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.NotToken:
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 6;
                default:
                    return 0;
            }
        }
        
        public static int GetBinaryPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.OrToken:
                    return 1;
                case SyntaxKind.AndToken:
                    return 2;
                case SyntaxKind.EqToken:
                case SyntaxKind.NEqToken:
                case SyntaxKind.GtEqToken:
                case SyntaxKind.LtEqToken:
                case SyntaxKind.GtToken:
                case SyntaxKind.LtToken:
                    return 3;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                case SyntaxKind.ModuloToken:
                    return 5;
                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeywordKind(string text)
        {
            return text switch
            {
                "true" => SyntaxKind.TrueKeyword,
                "false" => SyntaxKind.FalseKeyword,
                "and" => SyntaxKind.AndToken,
                "or" => SyntaxKind.OrToken,
                "not" => SyntaxKind.NotToken,
                "bool" => SyntaxKind.BoolToken,
                "int" => SyntaxKind.IntToken,
                "var" => SyntaxKind.VarToken,
                "do" => SyntaxKind.OBlockToken,
                "end" => SyntaxKind.CBlockToken,
                "if" => SyntaxKind.IfKeyword,
                "else" => SyntaxKind.ElseKeyword,
                "while" => SyntaxKind.WhileKeyword,
                "for" => SyntaxKind.ForKeyword,
                _ => SyntaxKind.IdentifierToken
            };
        }
        
        public static SyntaxKind GetSymbolKind(string s)
        {
            return s switch
            {
                "\0" => SyntaxKind.EndToken,
                "+" => SyntaxKind.PlusToken,
                "-" => SyntaxKind.MinusToken,
                "*" => SyntaxKind.StarToken,
                "/" => SyntaxKind.SlashToken,
                "%" => SyntaxKind.ModuloToken,
                "(" => SyntaxKind.OParenToken,
                ")" => SyntaxKind.CParenToken,
                "{" => SyntaxKind.OBlockToken,
                "}" => SyntaxKind.CBlockToken,
                ">" => SyntaxKind.GtToken,
                "<" => SyntaxKind.LtToken,
                "=" => SyntaxKind.AssignToken,
                "!" => SyntaxKind.NotToken,
                "||" => SyntaxKind.OrToken,
                "&&" => SyntaxKind.AndToken,
                "==" => SyntaxKind.EqToken,
                "!=" => SyntaxKind.NEqToken,
                ">=" => SyntaxKind.GtEqToken,
                "<=" => SyntaxKind.LtEqToken,
                "+=" => SyntaxKind.PlusAssignToken,
                "-=" => SyntaxKind.MinusAssignToken,
                "*=" => SyntaxKind.StarAssignToken,
                "/=" => SyntaxKind.SlashAssignToken,
                "%=" => SyntaxKind.ModuloAssignToken,
                _ => SyntaxKind.ErrorToken
            };
        }

        public static bool ExpectedLookahead(char current, char lookAhead)
        {
            switch (current)
            {
                default:
                    return false;
                case '+':
                case '-':
                case '*':
                case '/':
                case '%':
                case '!':
                case '>':
                case '<':
                case '=':
                    return lookAhead == '=';
                case '|':
                case '&':
                    return lookAhead == current;
            }
        }

        public static bool IsAssignment(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusAssignToken:
                case SyntaxKind.MinusAssignToken:
                case SyntaxKind.StarAssignToken:
                case SyntaxKind.SlashAssignToken:
                case SyntaxKind.ModuloAssignToken:
                case SyntaxKind.AssignToken:
                    return true;
                default:
                    return false;
            }
        }
    }
}