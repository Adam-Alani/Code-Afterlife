using System;
using System.Collections.Generic;
using System.Linq;
using AfterlifeInterpretor.CodeAnalysis;
using AfterlifeInterpretor.CodeAnalysis.Binding;
using AfterlifeInterpretor.CodeAnalysis.Syntax;

namespace AfterlifeInterpretor
{
    class Program
    {
        static void Main(string[] args)
        {
            string prompt;
            Interpretor interpretor = new Interpretor();
            do
            {
                Console.Write("> ");
                prompt = Console.ReadLine();
                switch (prompt)
                {
                    case "#multiline":
                        PromptMultiline(interpretor);
                        break;
                    case "#program":
                        PromptProgram(interpretor);
                        break;
                    case "#exit":
                        break;
                    default:
                        EvaluationResults evaluationResults = interpretor.Interpret(prompt);
                        if (evaluationResults.Errs.GetErrors().Any())
                        {
                            Console.ForegroundColor = ConsoleColor.Red;

                            foreach (Error error in evaluationResults.Errs.GetErrors())
                            {
                                Console.WriteLine(error);
                            }
                    
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine(evaluationResults.Value);
                        }
                        break;
                }
            } while (prompt != "#exit");
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
                Console.WriteLine(evaluationResults.Value);
            }
        }

        static void PromptMultiline(Interpretor interpretor)
        {
            List<string> lines = new List<string>();
            string prompt;

            do
            {
                Console.Write("| ");
                prompt = Console.ReadLine();
                if (prompt != "#end") lines.Add(prompt);
            } while (prompt != "#end");
            
            EvaluationResults ev = interpretor.Interpret(lines.ToArray())[^1];
            if (!ev.Errs.GetErrors().Any())
                Console.WriteLine(ev.Value);
        }
    }
}