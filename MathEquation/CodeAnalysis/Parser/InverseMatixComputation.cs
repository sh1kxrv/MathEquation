using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Parser
{
    public class InverseMatixComputation
    {
        protected readonly Calculator Calc = new Calculator();
        private double Determinant = 0;
        private Matrix.Matrix Matrix;
        private Determinant determinant;
        public InverseMatixComputation(Matrix.Matrix matrix)
        {
            Matrix = matrix;
            determinant = new Determinant(Matrix);
        }
        public Matrix.Matrix Calculate()
        {
            Determinant = determinant.Calculate();
            if (Determinant == 0)
                throw new Exception("Determinant = 0");
            if (Matrix.Order == 2)
                return Solve2x2();
            else if (Matrix.Order == 3)
                return Solve3x3();

            throw new Exception("4x4 5x5 6x6 | todo");
        }

        private Matrix.Matrix Solve3x3()
        {
            var inverse = new Matrix.Matrix(3,3);
            for (int i = 0; i < Matrix.Order; i++)
            {
                for (int j = 0; j < Matrix.Order; j++)
                {
                    var tempMatrix = Get2x2Matrix(i, j);
                    double det = new Determinant(tempMatrix).Calculate();
                    inverse[i][j] = Math.Pow(-1.0, i + j + 2) * det;
                }
            }
            return Transp(inverse) * (1.0 / Determinant);
        }
        private Matrix.Matrix Transp(Matrix.Matrix matrix)
        {
            var tempMatr = new Matrix.Matrix(3,3);
            for (int i = 0; i < Matrix.Order; i++)
                for (int j = 0; j < Matrix.Order; j++)
                    tempMatr[j][i] = matrix[i][j];
            return tempMatr;
        }
        //Вычеркивание столбцов и строк
        private Matrix.Matrix Get2x2Matrix(int row, int col)
        {
            var matrix = new Matrix.Matrix(2,2);
            int ki = 0;
            for (int i = 0; i < Matrix.Order; i++)
            {
                if (i != row)
                {
                    for (int j = 0, kj = 0; j < Matrix.Order; j++)
                    {
                        if (j != col)
                        {
                            matrix[ki][kj] = Matrix[i][j];
                            kj++;
                        }
                    }
                    ki++;
                }
            }
            return matrix;
        }
        private Matrix.Matrix Solve2x2()
        {
            return new Parser.Matrix.Matrix(2,2);
        }
    }
}
