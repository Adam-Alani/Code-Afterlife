using System;

namespace AfterlifeInterpretor.CodeAnalysis
{
    public class Text
    {
        public static void GetIndex(string text, int chr, out int line, out int pos)
        {
            line = 0;
            int lastLine = 0;
            for (int i = 0; i <= chr && i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    line++;
                    lastLine = i;
                }
            }

            pos = chr - lastLine;
        }

        public static string PrettyType(Type t)
        {
            if (t == typeof(int))
                return "int";
            if (t == typeof(string))
                return "string";
            if (t == typeof(double))
                return "float";
            if (t == typeof(List))
                return "list";
            if (t == typeof(Function))
                return "function";
            if (t == typeof(bool))
                return "bool";
            if (t == typeof(object))
                return "unpredictable";
            if (t == typeof(Unpredictable))
                return "unpredictable";
            return (t == null) ? "()" : t.Name;
        }
    }
}