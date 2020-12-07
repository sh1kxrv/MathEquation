using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathEquation.CodeAnalysis.Parser.Matrix;

namespace MathEquation.CodeAnalysis.Parser
{
    public class Determinant
    {
        protected Matrix.Matrix _matrix;
        protected readonly Calculator _calc = new Calculator();

        public Matrix.Matrix Matrix => _matrix;

        public Determinant(Matrix.Matrix matrix)
        {
            _matrix = matrix;
        }

        public double Calculate()
        {
            //if (Triangle(Matrix) && Matrix.Order == 2)
            //{
            //    for (int i = 0; i < Matrix.Rows; i++)
            //    {
            //        for (int j = 0; j < Matrix.Columns; j++)
            //        {
            //            if (Convert.ToDouble(Matrix[i][j]) != 0)
            //                return Convert.ToDouble(Matrix[i][j]);
            //        }
            //    }
            //}

            if (_matrix.Order == 1)
                return Convert.ToDouble(_matrix[0][0]);
            if (_matrix.Order == 2)
                return Calculate2x2();
            if (_matrix.Order == 3)
                return Calculate3x3();

            var result = 0.0;

            for (var i = 0; i < _matrix.Order; i++)
                result += _calc.Calculate($"(({IndMul(i, 0)})*({_matrix[i, 0]}))*({new Determinant(GetAlgebraicComplement(i, 0)).Calculate()})", "CalculateDeterminantMatrixError");

            return result;
        }

        private bool Triangle(Matrix.Matrix matr)
        {
            int k = 0, s = 0, o = 0;
            for (int i = 0; i < matr.Order; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (matr[i][j].ToString() == "0") s++;
                    if (matr[matr.Order - i - 1][matr.Order - j - 1].ToString() == "0") o++;
                    k++;
                }
            }
            return s == k || o == k;
        }
        public double Calculate2x2()
        {
            var exp = $"({_matrix[0, 0]})*({_matrix[1, 1]})-({_matrix[0, 1]})*({_matrix[1, 0]})";

            Calculator.InvokeOnMessage("#####\r\n[OUT] " + exp, 0);
            return _calc.Calculate(exp, "CalculateDeterminantMatrix2x2Error");
        }

        public double Calculate3x3()
        {
            var exp = $"(({_matrix[0, 0]})*({_matrix[1, 1]})*({_matrix[2, 2]}))+" +
                      $"(({_matrix[0, 1]})*({_matrix[2, 0]})*({_matrix[1, 2]}))+" +
                      $"(({_matrix[1, 0]})*({_matrix[0, 2]})*({_matrix[2, 1]}))";

            var exp2 = $"(({_matrix[2, 0]})*({_matrix[1, 1]})*({_matrix[0, 2]}))+" +
                       $"(({_matrix[0, 1]})*({_matrix[2, 2]})*({_matrix[1, 0]}))+" +
                       $"(({_matrix[0, 0]})*({_matrix[1, 2]})*({_matrix[2, 1]}))";

            Calculator.InvokeOnMessage("#####\r\n[OUT_1] " + exp, 0);
            var _c = _calc.Calculate(exp, "CalculateDeterminantMatrix3x3Error(TriangleRule) when evaluating the first expression");
            Calculator.InvokeOnMessage("#####\r\n[OUT_2] " + exp2, 0);
            var _c2 = _calc.Calculate(exp2, "CalculateDeterminantMatrix3x3Error(TriangleRule) when evaluating the first expression");

            return _c - _c2;
        }

        #region With Variables
        public string Calculate(string variables)
        {
            if (_matrix.Order == 2)
                return Calculate2x2(variables);
            if (_matrix.Order == 3)
                return Calculate3x3(variables);

            var result = "";

            for (var i = 0; i < _matrix.Order; i++)
            {
                var expr = $"(({IndMul(i, 0)})*({_matrix[i, 0]}))*({new Determinant(GetAlgebraicComplement(i, 0)).Calculate()})" + ";" + variables;
                var res = Calculator.CalculateUnknownString(expr, "CalculateDeterminantMatrixError");
                result += "+(" + res + ")";
                Calculator.InvokeOnLongTimeOperationReceiveMessage(expr + "=" + res);
            }

            Calculator.InvokeOnMessage("#####\r\n[OUT] " + result, 0);
            return Calculator.CalculateUnknownString(result);
        }

        public string Calculate2x2(string variables)
        {
            var exp = $"({_matrix[0, 0]})*({_matrix[1, 1]})-({_matrix[0, 1]})*({_matrix[1, 0]})";

            Calculator.InvokeOnMessage("#####\r\n[OUT] " + exp + ";" + variables, 0);
            return Calculator.CalculateUnknownString(exp + ";" + variables, "CalculateDeterminantMatrix2x2Error");
        }

        public string Calculate3x3(string variables)
        {
            var exp = $"(({_matrix[0, 0]})*({_matrix[1, 1]})*({_matrix[2, 2]}))+" +
                      $"(({_matrix[0, 1]})*({_matrix[2, 0]})*({_matrix[1, 2]}))+" +
                      $"(({_matrix[1, 0]})*({_matrix[0, 2]})*({_matrix[2, 1]}))";

            var exp2 = $"(({_matrix[2, 0]})*({_matrix[1, 1]})*({_matrix[0, 2]}))+" +
                       $"(({_matrix[0, 1]})*({_matrix[2, 2]})*({_matrix[1, 0]}))+" +
                       $"(({_matrix[0, 0]})*({_matrix[1, 2]})*({_matrix[2, 1]}))";

            Calculator.InvokeOnMessage("#####\r\n[OUT_1] " + exp + ";" + variables, 0);
            var _c = Calculator.CalculateUnknownString(exp + ";" + variables, "CalculateDeterminantMatrix3x3Error(TriangleRule) when evaluating the first expression");
            Calculator.InvokeOnMessage("#####\r\n[OUT_2] " + exp2 + ";" + variables, 0);
            var _c2 = Calculator.CalculateUnknownString(exp2 + ";" + variables, "CalculateDeterminantMatrix3x3Error(TriangleRule) when evaluating the first expression");

            return (Convert.ToDouble(_c) - Convert.ToDouble(_c2)).ToString();
        }
        #endregion

        protected Matrix.Matrix GetAlgebraicComplement(int x, int y)
        {
            var strs = new List<string>();

            for (var ym = 0; ym < _matrix.Order; ym++)
                for (var xm = 0; xm < _matrix.Order; xm++)
                    if (ym != y && xm != x)
                        strs.Add(_matrix[xm, ym]);

            Matrix.Matrix alComp = new Matrix.Matrix(_matrix.Order - 1, _matrix.Order - 1, strs.ToArray());

            return alComp;
        }

        protected int IndMul(int x, int y)
        {
            return IsEven(y * (_matrix.Order - 1) + x) ? 1 : -1;
        }

        protected bool IsEven(int a)
        {
            return (a & 1) == 0;
        }
    }
}
