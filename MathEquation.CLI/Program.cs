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

            var dtm = new DeterminantMatrix(5, 4, 1, 1, 2, 1,
                                                1, 2, -1, 1, 1,
                                                3, 1, 1, 1, 1,
                                                2, 1, 1, 4, 1,
                                                2, -1, 1, 1, 5);
            var dt = new Determinant(dtm);

            Console.WriteLine(dt.Calculate());

            //Console.WriteLine(new Calculator().Calculate("tg25"));
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
