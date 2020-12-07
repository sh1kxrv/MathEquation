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

            var inv2 = new InverseMatixComputation(new Matrix(2, 2, -2, 1, 1, 4));

            var inv3 = new InverseMatixComputation(new Matrix(5, 5, 5,7,-5,-2,-1,
                                                                 2,1,7,6,3,
                                                                 1,0,2,0,-3,
                                                                 5,0,-1,3,-4,
                                                                 -4,3,-2,0,4));

            Console.WriteLine("=======================");
            Console.WriteLine($"Inverse 3x3 Matrix:\n{inv.Calculate()}");
            Console.WriteLine($"Inverse 2x2 Matrix:\n{inv2.Calculate()}");
            Console.WriteLine($"Inverse 5x5 Matrix:\n{inv3.Calculate()}");

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
