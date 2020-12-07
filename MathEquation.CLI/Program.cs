using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathEquation.CodeAnalysis.Parser;
using MathEquation.CodeAnalysis.Parser.Matrix;

namespace MathEquation.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculator.OnError += (msg) => Console.WriteLine("{ERROR} " + msg);
            //Calculator.OnMessage += (msg, gen) => Console.WriteLine("{MESSAGE} " + msg + ", Generation: " + gen);
            //5*y^4-5*y^2+2=0;y=[-1000;1000;0.1]
            //(2*x)/(x+2)-(x-1)/(x-3)=10/((3-x)*(x+2));x=[]

            //Console.ForegroundColor = ConsoleColor.DarkGray;
            //while (true)
            //{
            //    Console.Write(">>> ");
            //    string line = Console.ReadLine();
            //    if (line.StartsWith("eq"))
            //    {
            //        ColoredWrite(ConsoleColor.DarkGray, "-> ");
            //        ColoredWriteLine(ConsoleColor.DarkYellow, $"{new Equation().CalculateX(line.Replace("eq", ""))}");
            //    }
            //    else
            //    {
            //        ColoredWrite(ConsoleColor.DarkGray, "-> ");
            //        ColoredWriteLine(ConsoleColor.DarkYellow, Calculator.CalculateUnknownString(line));
            //    }
            //}


            //FFUUUUUUUUUUUCK
            //Console.WriteLine(new Calculator().Calculate("2*(atg90)"));

            var dtm = new Matrix(3, 3, "(-3.3)",  3,  -1,
                                       4,  1,  "(1234567891011)",
                                       1, -2, -2);

            var dtm2 = new Matrix(3, 1, -1, 2, 1,
                                         3, 1, 1,
                                        -2, 1, 1);

            //Console.WriteLine(dtm * dtm2);

            var dt = new Determinant_WithSteps(dtm);

            Console.WriteLine(dt.Calculate_WithVarsSteps("", 0).Item2 + "\r\n=========================================\r\n");
            Console.WriteLine(dt.Calculate_WithVarsSteps("", 1).Item2 + "\r\n=========================================\r\n");
            Console.WriteLine(dt.Calculate_WithVarsSteps("", 2).Item2 + "\r\n=========================================\r\n");

            var inv = new InverseMatixComputation(new Matrix(3,3, 5, -2, 1,
                                                                  2, 1, 4,
                                                                  3, 1, 2));
            Console.WriteLine("=======================");
            Console.WriteLine($"Inverse Matrix:\n{inv.Calculate()}");

            //Console.WriteLine(new Calculator().Calculate("(-25.04)"));
            //Console.WriteLine(new Calculator().Calculate("((2+3)!-3!)/3!"));

            Console.ReadLine();
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
