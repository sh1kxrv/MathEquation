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
            Console.WriteLine(new Equation().CalculateX("2*(2*(2+x))=4"));

            Console.ReadLine();
        }
    }
}
