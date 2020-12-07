using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Parser.Matrix
{
    public class Matrix : List<List<object>>
    {
        public int Rows => this.Count;
        public int Columns => Rows == 0 ? 0 : this[0].Count;

        [Obsolete("Order is deprecated, use Rows adn Columns instead.")]
        public int Order => Rows;

        public Matrix() : base()
        {
        }

        public Matrix(int rows_count, int columns_count, params object[] expressions)
        {
            //if (expressions.Length != order * order)
            //{
            //    throw new Exception($"The number of expressions({expressions.Length}) must be equal to the number of cells({order * order}) in the matrix");
            //}

            var dft = expressions.Length == 0 ? 0 : expressions[0];

            for (var y = 0; y < rows_count; y++)
            {
                var line = new List<object>();

                for (var x = columns_count * y; x < columns_count * y + columns_count; x++)
                    if (expressions.Length <= x)
                        line.Add(dft);
                    else
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


        private static Calculator _calc = new Calculator();

        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (left.Rows != right.Rows ||
                left.Columns != right.Columns)
            {
                throw new Exception("Columns and Rows must be equals");
            }

            List<object> args = new List<object>();
            for (var y = 0; y < left.Rows; y++)
            {
                for (var x = 0; x < right.Columns; x++)
                {
                    args.Add(_calc.Calculate($"({left[x, y]}) + ({right[x, y]})"));
                }
            }

            return new Matrix(left.Rows, right.Columns, args.ToArray());
        }

        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (left.Rows != right.Rows ||
                left.Columns != right.Columns)
            {
                throw new Exception("Columns and Rows must be equals");
            }

            List<object> args = new List<object>();
            for (var y = 0; y < left.Rows; y++)
            {
                for (var x = 0; x < right.Columns; x++)
                {
                    args.Add(_calc.Calculate($"({left[x, y]}) - ({right[x, y]})"));
                }
            }

            return new Matrix(left.Rows, right.Columns, args.ToArray());
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (left.Columns != right.Rows)
                throw new Exception("left.Columns != right.Rows");

            var matrixC = new Matrix(left.Rows, right.Columns);

            for (var i = 0; i < left.Rows; i++)
            {
                for (var j = 0; j < right.Columns; j++)
                {
                    var tmp = 0.0;

                    for (var k = 0; k < left.Columns; k++)
                    {
                        tmp += _calc.Calculate($"({left[k, i]}) * ({right[j, k]})");
                    }

                    matrixC[j, i] = tmp.ToString();
                }
            }

            return matrixC;
        }

        public static Matrix operator *(Matrix left, double variable)
        {
            var calculated = new Matrix(left.Rows, left.Columns);
            for (int i = 0; i < left.Rows; i++)
                for (int j = 0; j < left.Columns; j++)
                    calculated[i][j] = _calc.Calculate($"({left[i][j]})*({variable})");
            return calculated;
        }

        public Matrix Resize(int rows, int columns)
        {
            var result = new Matrix(rows, columns, "");

            var miny = rows < this.Rows ? rows : this.Rows;
            var minx = columns < this.Columns ? columns : this.Columns;

            for (var y = 0; y < miny; y++)
            {
                for (var x = 0; x < minx; x++)
                {
                    result[x, y] = this[x, y];
                }
            }

            return result;
        }

        public bool IsAnyCellEmpty()
        {
            foreach (var row in this)
                foreach (var cell in row)
                    if (string.IsNullOrEmpty(cell?.ToString()))
                        return true;
            return false;
        }

        public override string ToString()
        {
            var cols = new List<int>();

            while (cols.Count < this.Columns)
                cols.Add(0);

            for (var y = 0; y < this.Rows; y++)
                for (var x = 0; x < this.Columns; x++)
                    if (cols[x] < this[x, y].Length)
                        cols[x] = this[x, y].Length;

            return string.Join("\r\n", this.Select((arr, x) => {
                var str = "| ";
                str += string.Join("  ", arr.Select((cell, y) =>
                {
                    return $"a{x + 1}{y + 1}={cell}" + new string(' ', cols[y] - cell.ToString().Length);
                }));
                str += " |";
                return str;
            }));
        }
    }
}
