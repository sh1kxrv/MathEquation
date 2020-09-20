using MathEquation.CodeAnalysis.Parser;
using MathEquation.CodeAnalysis.Parser.Syntax;
using MathEquation.CodeAnalysis.Parser.Syntax.Evaluator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            while (true)
            {
                Console.Write(">>> ");
                string line = Console.ReadLine();
                if (line.StartsWith("eq "))
                {
                    var eq = new EquationEvaluator(line.Replace("eq ", ""));
                    double result = eq.Calculate();
                    if (eq.LexerErrors.Any())
                    {
                        foreach (var error in eq.LexerErrors)
                            ColoredWriteLine(ConsoleColor.Red, error);
                    }
                    else
                    {
                        ColoredWrite(ConsoleColor.DarkGray, "Result of equation is: ");
                        ColoredWriteLine(ConsoleColor.DarkYellow, result.ToString());
                    }
                }
                else
                {
                    MathParser parser = new MathParser(line);
                    var node = parser.Parse().Root;
                    PrintTree(node);
                    if (parser.Errors.Any())
                    {
                        foreach (var error in parser.Errors)
                            ColoredWriteLine(ConsoleColor.Red, error);
                    }
                    else
                    {
                        MathEvaluator evaluator = new MathEvaluator(node);
                        ColoredWrite(ConsoleColor.DarkGray, "Result of expression is: ");
                        ColoredWriteLine(ConsoleColor.DarkYellow, evaluator.Evaluate().ToString());
                    }
                }
                //MathParser parser = new MathParser(line);
                //var node = parser.Parse().Root;
                //PrintTree(node);

                //if (parser.Errors.Any())
                //{
                //    foreach(var error in parser.Errors)
                //        ColoredWriteLine(ConsoleColor.Red, error);
                //}
                //else
                //{
                //MathEvaluator evaluator = new MathEvaluator(node);
                //ColoredWrite(ConsoleColor.DarkGray, "Result of expression is: ");
                //ColoredWriteLine(ConsoleColor.DarkYellow, evaluator.Evaluate().ToString());
                //evaluate
                //}
            }
        }
        private static void PrintTree(SyntaxNode expression, string indent = "")
        {
            Console.Write(indent);
            ColoredWrite(ConsoleColor.DarkGray, expression.Kind.ToString());

            if (expression is SyntaxToken t && t.Value != null)
            {
                Console.Write(" -> ");
                ColoredWrite(ConsoleColor.DarkYellow, t.Value.ToString());
            }

            Console.WriteLine();

            indent += '\t';

            foreach (var child in expression.GetChildren())
                PrintTree(child, indent);
        }
        public static void ColoredWrite(ConsoleColor color, string msg)
        {
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }
        public static void ColoredWriteLine(ConsoleColor color, string msg)
        {
            ColoredWrite(color, msg + "\n");
        }
    }
}
