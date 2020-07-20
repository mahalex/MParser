using System.Text;

namespace Parser.Objects
{
    public class MDoubleMatrix : MObject
    {
        private MDoubleMatrix(double[,] matrix)
        {
            Matrix = matrix;
            RowCount = matrix.GetLength(0);
            ColumnCount = matrix.GetLength(1);
        }

        public double[,] Matrix { get; }

        public int RowCount { get; }

        public int ColumnCount { get; }

        public ref double this[int i, int j] => ref Matrix[i, j];

        public ref double this[int i] => ref Matrix[i % RowCount, i / RowCount];

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < RowCount; i++)
            {
                for (var j = 0; j < ColumnCount; j++)
                {
                    if (j > 0)
                    {
                        sb.Append(' ');
                    }
                    sb.Append(Matrix[i, j]);
                }

                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static MDoubleMatrix Create(double[,] matrix)
        {
            return new MDoubleMatrix(matrix);
        }
    }
}