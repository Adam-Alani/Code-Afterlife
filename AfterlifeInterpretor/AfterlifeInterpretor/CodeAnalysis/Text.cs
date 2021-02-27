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
    }
}