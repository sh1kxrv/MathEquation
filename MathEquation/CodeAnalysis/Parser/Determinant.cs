using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Parser
{
    public class Determinant
    {
        private DeterminantMatrix _matrix;
        private Calculator _calc = new Calculator();

        public Determinant(DeterminantMatrix matrix)
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

        public double Calculate2x2()
        {
            var exp = $"({_matrix[0, 0]})*({_matrix[1, 1]})-({_matrix[0, 1]})*({_matrix[1, 0]})";

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

            var _c = _calc.Calculate(exp, "CalculateDeterminantMatrix3x3Error(TriangleRule) when evaluating the first expression");

            var _c2 = _calc.Calculate(exp2, "CalculateDeterminantMatrix3x3Error(TriangleRule) when evaluating the first expression");

            return _c - _c2;
        }

        private Determinant GetAlgebraicComplement(int x, int y)
        {
            var strs = new List<string>();

            for (var ym = 0; ym < _matrix.Order; ym++)
                for (var xm = 0; xm < _matrix.Order; xm++)
                    if (ym != y && xm != x)
                        strs.Add(_matrix[xm, ym]);

            DeterminantMatrix alComp = new DeterminantMatrix(_matrix.Order - 1, strs.ToArray());

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

    public class DeterminantMatrix : List<List<object>>
    {
        public int Order;

        public DeterminantMatrix(int order, params object[] expressions)
        {
            if (expressions.Length != order * order)
            {
                throw new Exception($"The number of expressions({expressions.Length}) must be equal to the number of cells({order * order}) in the matrix");
            }

            Order = order;

            for (var y = 0; y < order; y++)
            {
                var line = new List<object>();

                for (var x = order * y; x < order * y + order; x++)
                    line.Add(expressions[x]);

                this.Add(line);
            }
        }

        public string this[int x, int y]
        {
            get
            {
                var obj = this[y][x];
                return obj.ToString();
            }
            set
            {
                this[y][x] = value;
            }
        }
    }
}
