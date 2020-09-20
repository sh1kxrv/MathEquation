using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathEquation.CodeAnalysis.Parser;

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
                if (line.StartsWith("eq"))
                {
                    ColoredWrite(ConsoleColor.DarkGray, "-> ");
                    ColoredWriteLine(ConsoleColor.DarkYellow, $"{new Equation().CalculateX(line.Replace("eq", ""))}");
                }
                else
                {
                    ColoredWrite(ConsoleColor.DarkGray, "-> ");
                    ColoredWriteLine(ConsoleColor.DarkYellow, $"{new Calculator().Calculate(line)}");
                }
            }
            //FFUUUUUUUUUUUCK
            /*Console.WriteLine(new Calculator().Calculate("2*(atg90)"));

            var dtm = new DeterminantMatrix(5,  4,  1,  1,  2,  1,
                                                1,  2, -1,  1,  1,
                                                3,  1,  1,  1,  1,
                                                2,  1,  1,  4,  1,
                                                2, -1,  1,  1,  5);
            var dt = new Determinant(dtm);

            Console.WriteLine(dt.Calculate());*/

            //Console.WriteLine(new Calculator().Calculate("tg25"));
            //Console.WriteLine(new Calculator().Calculate("5,52 + 5.15"));
        }
        private static void ColoredWrite(ConsoleColor color, string msg)
        {
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ForegroundColor = ConsoleColor.DarkGray;
        }
        private static void ColoredWriteLine(ConsoleColor color, string msg)
        {
            ColoredWrite(color, msg + "\n");
        }
    }
}
