using MathEquation.CodeAnalysis.Lexer.Syntax;
using MathEquation.CodeAnalysis.Lexer.Syntax.Expressions;
using MathEquation.CodeAnalysis.Parser;
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
                MathParser parser = new MathParser(line);
                var expression = parser.Parse();
                PrintTree(expression);
            }
        }
        private static void PrintTree(SyntaxNode expression, string indent = "")
        {
            Console.Write(indent);
            Console.Write(expression.Kind);

            if(expression is SyntaxToken t && t.Value != null)
                Console.WriteLine(" " + ((SyntaxToken)expression).Value);
            else
                Console.WriteLine();

            indent += '\t';

            foreach (var child in expression.GetChildren())
                PrintTree(child, indent);
        }
    }
}
