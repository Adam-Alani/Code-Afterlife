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
                case SyntaxKind.PrintToken:
                    return 8;
                case SyntaxKind.SizeToken:
                case SyntaxKind.TailToken:
                case SyntaxKind.HeadToken:
                case SyntaxKind.NotToken:
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 7;
                default:
                    return 0;
            }
        }
        
        public static int GetBinaryPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.CommaToken:
                    return 1;
                case SyntaxKind.OrToken:
                    return 2;
                case SyntaxKind.AndToken:
                    return 3;
                case SyntaxKind.EqToken:
                case SyntaxKind.NEqToken:
                case SyntaxKind.GtEqToken:
                case SyntaxKind.LtEqToken:
                case SyntaxKind.GtToken:
                case SyntaxKind.LtToken:
                    return 4;
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 5;
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                case SyntaxKind.DoubleSlashToken:
                case SyntaxKind.ModuloToken:
                    return 6;
                case SyntaxKind.DotToken:
                    return 8;
                default:
                    return 0;
            }
        }

        public static bool IsRightAssociative(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.DotToken:
                case SyntaxKind.CommaToken:
                    return false;
                default:
                    return true;
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
                "string" => SyntaxKind.StringToken,
                "float" => SyntaxKind.FloatToken,
                "list" => SyntaxKind.ListToken,
                "var" => SyntaxKind.VarToken,
                "do" => SyntaxKind.OBlockToken,
                "begin" => SyntaxKind.OBlockToken,
                "end" => SyntaxKind.CBlockToken,
                "if" => SyntaxKind.IfKeyword,
                "else" => SyntaxKind.ElseKeyword,
                "while" => SyntaxKind.WhileKeyword,
                "for" => SyntaxKind.ForKeyword,
                "head" => SyntaxKind.HeadToken,
                "tail" => SyntaxKind.TailToken,
                "size" => SyntaxKind.SizeToken,
                "print" => SyntaxKind.PrintToken,
                _ => SyntaxKind.IdentifierToken
            };
        }
        
        public static SyntaxKind GetSymbolKind(string s)
        {
            return s switch
            {
                "\0" => SyntaxKind.EndToken,
                "," => SyntaxKind.CommaToken,
                "." => SyntaxKind.DotToken,
                "+" => SyntaxKind.PlusToken,
                "-" => SyntaxKind.MinusToken,
                "*" => SyntaxKind.StarToken,
                "/" => SyntaxKind.SlashToken,
                "//" => SyntaxKind.DoubleSlashToken,
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
                case '/':
                    return lookAhead == '=' || lookAhead == current;
                case '+':
                case '-':
                case '*':
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