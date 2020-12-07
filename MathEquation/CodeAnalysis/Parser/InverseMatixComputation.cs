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
            //if(Matrix.Order > 3)
            //    throw new Exception("Support only 2x2, 3x3");
            Determinant = determinant.Calculate();
            if (Determinant == 0)
                throw new Exception("Determinant = 0");
            return Solve();
        }

        private Matrix.Matrix Solve()
        {
            var inverse = new Matrix.Matrix(Matrix.Rows, Matrix.Columns);
            for (int i = 0; i < Matrix.Order; i++)
            {
                for (int j = 0; j < Matrix.Order; j++)
                {
                    var tempMatrix = GetTempMatrix(i, j, Matrix.Order - 1);
                    double det = new Determinant(tempMatrix).Calculate();
                    inverse[i][j] = Math.Pow(-1.0, i + j + 2) * det;
                }
            }
            return Transp(inverse) * (1.0 / Determinant);
        }
        private Matrix.Matrix Transp(Matrix.Matrix matrix)
        {
            var tempMatr = new Matrix.Matrix(matrix.Rows, matrix.Columns);
            for (int i = 0; i < Matrix.Order; i++)
                for (int j = 0; j < Matrix.Order; j++)
                    tempMatr[j][i] = matrix[i][j];
            return tempMatr;
        }
        //Вычеркивание столбцов и строк
        private Matrix.Matrix GetTempMatrix(int row, int col, int order)
        {
            var matrix = new Matrix.Matrix(order, order);
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
    }
}
