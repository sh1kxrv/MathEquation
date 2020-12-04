using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Parser.Matrix
{
    public class Matrix : List<List<object>>
    {
        public int Rows;
        public int Columns;

        [Obsolete("Order is deprecated, use Rows adn Columns instead.")]
        public int Order => Rows;

        public Matrix(int rows_count, int columns_count, params object[] expressions)
        {
            //if (expressions.Length != order * order)
            //{
            //    throw new Exception($"The number of expressions({expressions.Length}) must be equal to the number of cells({order * order}) in the matrix");
            //}

            Rows = rows_count;
            Columns = columns_count;

            for (var y = 0; y < rows_count; y++)
            {
                var line = new List<object>();

                for (var x = columns_count * y; x < columns_count * y + columns_count; x++)
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

        //TODO egor
        public static Matrix operator +(Matrix left, Matrix right)
        {
            throw new NotSupportedException();
        }

        public static Matrix operator -(Matrix left, Matrix right)
        {
            throw new NotSupportedException();
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            throw new NotSupportedException();
        }
    }
}
