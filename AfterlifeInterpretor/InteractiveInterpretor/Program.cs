using System;
using System.Collections.Generic;
using System.Linq;
using AfterlifeInterpretor;
using AfterlifeInterpretor.CodeAnalysis;

namespace InteractiveInterpretor
{
    class Program
    {
        private static readonly Dictionary<string, string> Macros = new Dictionary<string, string>();
        
        static void Main()
        {
            string prompt;
            Interpretor interpretor = new Interpretor();
            do
            {
                Console.Write("> ");
                prompt = Console.ReadLine() ?? "";
                if (prompt.StartsWith("#macro"))
                {
                    Macro(prompt.Split(' ')[1]);
                    continue;
                }
                switch (prompt)
                {
                    case "#program":
                        PromptProgram(interpretor);
                        break;
                    case "#reset":
                        interpretor = new Interpretor();
                        break;
                    case "#exit":
                        break;
                    default:
                        Interpret(interpretor, prompt);
                        break;
                }
            } while (prompt != "#exit");
        }

        private static void Macro(string name)
        {
            string text = "";
            string prompt;

            do
            {
                Console.Write("| ");
                prompt = Console.ReadLine();
                if (prompt != "#end") text += $"{prompt}\n";
            } while (prompt != "#end");
            
            Macros[name] = text;
        }

        private static void PromptProgram(Interpretor interpretor)
        {
            string text = "";
            string prompt;

            do
            {
                Console.Write("| ");
                prompt = Console.ReadLine();
                if (prompt != "#end") text += $"{prompt}\n";
            } while (prompt != "#end");
            
            Interpret(interpretor, text);
        }

        static void Interpret(Interpretor interpretor, string text)
        {
            if (text.StartsWith("#") && Macros.ContainsKey(text.Substring(1)))
                text = Macros[text.Substring(1)];
            EvaluationResults evaluationResults = interpretor.Interpret(text);
            if (evaluationResults.Errs.GetErrors().Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;

                foreach (Error error in evaluationResults.Errs.GetErrors())
                {
                    Text.GetIndex(text, error.Position, out int line, out int pos);
                    Console.WriteLine(error.ToString(line, pos));
                }
                    
                Console.ResetColor();
            }
            else
            {

                if (evaluationResults.StdOut.Length > 0)
                {
                    Console.WriteLine(evaluationResults.StdOut);
                    Console.WriteLine();
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(evaluationResults.Value);
                Console.ResetColor();
            }
        }
    }
}