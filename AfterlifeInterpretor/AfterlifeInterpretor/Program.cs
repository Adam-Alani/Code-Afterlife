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
            
            do
            {
                Console.Write("> ");
                prompt = Console.ReadLine();
                if (prompt != "exit")
                {
                    Parser parser = new Parser(prompt);
                    SyntaxTree tree = parser.Parse();
                    Binder binder = new Binder();
                    BoundExpression root = binder.BindExpression(tree.Root);

                    string[] enumerable = tree.Errors.Concat(binder.Errors).ToArray();
                    if (enumerable.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;

                        foreach (string error in enumerable)
                        {
                            Console.WriteLine(error);
                        }
                    
                        Console.ResetColor();
                    }
                    else
                    {
                        Evaluator e = new Evaluator(root);
                        object r = e.Evaluate();
                        Console.WriteLine(r);
                    }
                }
            } while (prompt != "exit");
        }
    }
}