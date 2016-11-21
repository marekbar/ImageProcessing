namespace ImageProcessing.Filters
{
    public static class FilterFactory
    {
        public static FilterMatrix AverageFilter
        {
            get
            {
                return new FilterMatrix(new double[,]
                {
                    {1,1,1 },
                    {1,1,1 },
                    {1,1,1 },
                });
            }
        }

        public static FilterMatrix Sharpen
        {
            get
            {
                return new FilterMatrix(new double[,]
                {
                    { 0 , -2 , 0 },
                    { -2 , 11 , -2 },
                    { 0 , -2 , 0 }
                });
            }
        }

        public static FilterMatrix EdgesSobelHorizontal
        {
            get
            {
                return new FilterMatrix(new double[,]
             {
                 { 1,   2,   1 },
                    { 0,   0,   0 },
                    { -1,  -2,  -1 }
            });
            }
        }

        public static FilterMatrix EmbossEast
        {
            get
            {
                return new FilterMatrix(new double[,]
             {
                 { -1,   0,   1 },
                    { -1,   0,   1 },
                    { -1,  0,  1 }
            });
            }
        }
    }
}
