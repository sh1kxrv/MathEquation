using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathEquation.CodeAnalysis.Parser.Matrix;

namespace MathEquation.CodeAnalysis.Parser
{
    public class Determinant_WithSteps : Determinant
    {
        public Determinant_WithSteps(Matrix.Matrix matrix) : base(matrix)
        {
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
            if (way < 0 || way > 2)
                way = 0;

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
                var res = new Determinant_WithSteps(GetAlgebraicComplement(i, 0));
                var res2 = res.Calculate_WithSteps(way);
                result.Item2 += $"(({IndMul(i, 0)})*({_matrix[i, 0]}))*\r\n{res.Matrix} {((i == _matrix.Order - 1) ? "=\r\n" : "+\r\n")} \r\n";
                result.Item1 += _calc.Calculate($"(({IndMul(i, 0)})*({_matrix[i, 0]}))*({res2.Item1})", "CalculateDeterminantMatrixError");
            }

            result.Item2 += result.Item1;

            return result;
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

            var cols = new List<int>();
            while (cols.Count < _matrix.Columns)
                cols.Add(0);
            for (var y = 0; y < _matrix.Rows; y++)
                for (var x = 0; x < _matrix.Columns; x++)
                    if (cols[x] < _matrix[x, y].Length)
                        cols[x] = _matrix[x, y].Length;

            result.Item2 = "1.\r\n" +
            string.Join("\r\n", _matrix.Select((arr, x) => {
                var str = "| ";
                str += string.Join("  ", arr.Select((cell, y) =>
                {
                    return $"a{x + 1}{y + 1}={cell}" + new string(' ', cols[y] - cell.ToString().Length);
                }));
                str += " | \t" + $"a{x + 1}1={arr[0]}{new string(' ', cols[0] - arr[0].ToString().Length)}  " + $"a{x + 1}2={arr[1]}{new string(' ', cols[1] - arr[1].ToString().Length)} |";
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
        public (double, string) Calculate_WithVarsSteps(string variables, int way = 0)
        {
            if (!string.IsNullOrEmpty(variables))
                return (double.Parse(Calculate(variables)), "При использовании переменных информация не доступна");

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
                var res = new Determinant_WithSteps(GetAlgebraicComplement(i, 0));
                var res2 = res.Calculate_WithSteps(way);
                result.Item2 += $"(({IndMul(i, 0)})*({_matrix[i, 0]}))*\r\n{res.Matrix} {((i == _matrix.Order - 1) ? "=\r\n" : "+\r\n")} \r\n";
                result.Item1 += _calc.Calculate($"(({IndMul(i, 0)})*({_matrix[i, 0]}))*({res2.Item1})", "CalculateDeterminantMatrixError");
            }

            result.Item2 += result.Item1;

            return result;
        }
        #endregion
    }
}
