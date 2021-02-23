using System;
using System.Linq;
using AfterlifeInterpretor.CodeAnalysis;

namespace AfterlifeInterpretor
{
    class Program
    {
        static void Main(string[] args)
        {
            string prompt;
            
            do
            {
                Console.Write("> ");
                prompt = Console.ReadLine();
                Parser parser = new Parser(prompt);
                SyntaxTree tree = parser.Parse();

                if (tree.Errors.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    foreach (string error in tree.Errors)
                    {
                        Console.WriteLine(error);
                    }
                    
                    Console.ResetColor();
                }
                else
                {
                    Evaluator e = new Evaluator(tree.Root);
                    int r = e.Evaluate();
                    Console.WriteLine(r);
                }

            } while (prompt != "exit");
        }
    }
}