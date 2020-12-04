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
        private Matrix.Matrix _matrix;
        private Calculator _calc = new Calculator();

        public Matrix.Matrix Matrix => _matrix;

        public Determinant(Matrix.Matrix matrix)
        {
            _matrix = matrix;
        }

        public double Calculate()
        {
            if (_matrix.Order == 2)
                return Calculate2x2();
            if (_matrix.Order == 3)
                return Calculate3x3();

            var result = 0.0;

            for (var i = 0; i < _matrix.Order; i++)
                result += _calc.Calculate($"(({IndMul(i, 0)})*({_matrix[i, 0]}))*({GetAlgebraicComplement(i, 0).Calculate()})", "CalculateDeterminantMatrixError");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="way">
        ///     0 - TriangleRule
        ///     1 - SarrusRule
        ///     2 - Разложение по !
        /// </param>
        /// <returns>
        ///     Item1 - response
        ///     Item2 - steps
        /// </returns>
        public (double, string) Calculate_WithSteps(int way = 0)
        {
            if (_matrix.Order == 2)
                return (Calculate2x2(), "");

            var result = (0.0, "");

            if (_matrix.Order == 3)
                switch (way)
                {
                    case 0: return Calculate3x3_TriangleRule();
                    case 1: return Calculate3x3_SarrusRule();
                }

            result.Item2 += "1.\r\n" + _matrix.ToString() + " = ";
            result.Item2 += "\r\n\r\n2.\r\n";

            for (var i = 0; i < _matrix.Order; i++)
            {
                var res = GetAlgebraicComplement(i, 0);
                var res2 = res.Calculate_WithSteps(way);
                result.Item2 += $"(({IndMul(i, 0)})*({_matrix[i, 0]}))*\r\n{res.Matrix} {((i == _matrix.Order - 1) ? "=\r\n" : "+\r\n")} \r\n";
                result.Item1 += _calc.Calculate($"(({IndMul(i, 0)})*({_matrix[i, 0]}))*({res2.Item1})", "CalculateDeterminantMatrixError");
            }

            result.Item2 += result.Item1;

            return result;
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

        public (double, string) Calculate3x3_TriangleRule()
        {
            (double, string) result;

            result.Item2 = "1.\r\n" + _matrix.ToString() + " = ";

            var exp = $"(({_matrix[0, 0]})*({_matrix[1, 1]})*({_matrix[2, 2]}))+" +
                      $"(({_matrix[0, 1]})*({_matrix[1, 2]})*({_matrix[2, 0]}))+" +
                      $"(({_matrix[0, 2]})*({_matrix[1, 0]})*({_matrix[2, 1]}))";

            var exp2 = $"(({_matrix[0, 0]})*({_matrix[1, 2]})*({_matrix[2, 1]}))+" +
                       $"(({_matrix[0, 1]})*({_matrix[1, 0]})*({_matrix[2, 2]}))+" +
                       $"(({_matrix[0, 2]})*({_matrix[1, 1]})*({_matrix[2, 0]}))";

            result.Item2 += "\r\n\r\n2.\r\n" + exp + " - \r\n" + exp2 + " = ";

            Calculator.InvokeOnMessage("#####\r\n[OUT_1] " + exp, 0);
            var _c = _calc.Calculate(exp, "CalculateDeterminantMatrix3x3Error(TriangleRule) when evaluating the first expression");
            Calculator.InvokeOnMessage("#####\r\n[OUT_2] " + exp2, 0);
            var _c2 = _calc.Calculate(exp2, "CalculateDeterminantMatrix3x3Error(TriangleRule) when evaluating the second expression");

            result.Item1 = _c - _c2;

            result.Item2 += "\r\n\r\n3.\r\n(" + _c + ") - (" + _c2 + ") = " + result.Item1;

            return result;
        }

        public (double, string) Calculate3x3_SarrusRule()
        {
            (double, string) result;

            result.Item2 = "1.\r\n" +
            string.Join("\r\n", _matrix.Select((arr, x) => {
                var str = "| ";
                str += string.Join(" ", arr.Select((cell, y) =>
                {
                    return $"a{x + 1}{y + 1}={cell}\t";
                }));
                str += " | \t" + $"a{x + 1}1={arr[0]}\t" + $"a{x + 1}2={arr[1]}\t |";
                return str;
            })) + " = ";

            var exp = $"(({_matrix[0, 0]})*({_matrix[1, 1]})*({_matrix[2, 2]}))+" +
                      $"(({_matrix[0, 1]})*({_matrix[1, 2]})*({_matrix[2, 0]}))+" +
                      $"(({_matrix[0, 2]})*({_matrix[1, 0]})*({_matrix[2, 1]}))";

            var exp2 = $"(({_matrix[0, 2]})*({_matrix[1, 1]})*({_matrix[2, 0]}))+" +
                       $"(({_matrix[0, 0]})*({_matrix[1, 2]})*({_matrix[2, 1]}))+" +
                       $"(({_matrix[0, 1]})*({_matrix[1, 0]})*({_matrix[2, 2]}))";

            result.Item2 += "\r\n\r\n2.\r\n" + exp + " - \r\n" + exp2 + " = ";

            Calculator.InvokeOnMessage("#####\r\n[OUT_1] " + exp, 0);
            var _c = _calc.Calculate(exp, "CalculateDeterminantMatrix3x3Error(TriangleRule) when evaluating the first expression");
            Calculator.InvokeOnMessage("#####\r\n[OUT_2] " + exp2, 0);
            var _c2 = _calc.Calculate(exp2, "CalculateDeterminantMatrix3x3Error(TriangleRule) when evaluating the second expression");

            result.Item1 = _c - _c2;

            result.Item2 += "\r\n\r\n3.\r\n(" + _c + ") - (" + _c2 + ") = " + result.Item1;

            return result;
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
                var expr = $"(({IndMul(i, 0)})*({_matrix[i, 0]}))*({GetAlgebraicComplement(i, 0).Calculate()})" + ";" + variables;
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


        private Determinant GetAlgebraicComplement(int x, int y)
        {
            var strs = new List<string>();

            for (var ym = 0; ym < _matrix.Order; ym++)
                for (var xm = 0; xm < _matrix.Order; xm++)
                    if (ym != y && xm != x)
                        strs.Add(_matrix[xm, ym]);

            Matrix.Matrix alComp = new Matrix.Matrix(_matrix.Order - 1, _matrix.Order - 1, strs.ToArray());

            return new Determinant(alComp);
        }

        private int IndMul(int x, int y)
        {
            return IsEven(y * (_matrix.Order - 1) + x) ? 1 : -1;
        }

        private bool IsEven(int a)
        {
            return (a & 1) == 0;
        }
    }
}
