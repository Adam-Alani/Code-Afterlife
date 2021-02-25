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
                if (prompt != "exit")
                {
                    EvaluationResults evaluationResults = interpretor.Evaluate(prompt);
                    if (evaluationResults.Errors.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;

                        foreach (string error in evaluationResults.Errors)
                        {
                            Console.WriteLine(error);
                        }
                    
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(evaluationResults.Value);
                    }
                }
            } while (prompt != "exit");
        }
    }
}