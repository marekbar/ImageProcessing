namespace ImageProcessing.Filters
{
    public class FilterMatrix
    {

        public double Factor { get; set; }
        public int Offset
        {
            get
            {
                return (Width - 1) / 2;
            }
        }

        private double[,] matrix;

        public double this[int x, int y]
        {
            get
            {
                return matrix[x, y];
            }
        }

        public int Width
        {
            get
            {
                return matrix.GetLength(1);
            }
        }

        public int Height
        {
            get
            {
                return matrix.GetLength(0);
            }
        }
        public int Size { get; internal set; }

        public FilterMatrix(double[,] matrix)
        {
            this.matrix = matrix;
            double sum = 0;
            Size = matrix.GetLength(0);
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    sum += matrix[i, j];

            if (sum == 0)
                sum = 1;
            Factor = 1 / sum;
        }
      
    }
}
