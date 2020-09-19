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
            //FFUUUUUUUUUUUCK
            Console.WriteLine(new Equation().CalculateX("x+x+22=64-2"));

            var dtm = new DeterminantMatrix(5,  4,  1,  1,  2,  1,
                                                1,  2, -1,  1,  1,
                                                3,  1,  1,  1,  1,
                                                2,  1,  1,  4,  1,
                                                2, -1,  1,  1,  5);
            var dt = new Determinant(dtm);

            Console.WriteLine(dt.Calculate());

            Console.ReadLine();
        }
    }
}
