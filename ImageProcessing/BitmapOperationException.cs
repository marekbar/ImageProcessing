using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    public class BitmapOperationException : Exception
    {
        public string OperationName { get; private set; }
        public bool OperationIsNotSupported { get; private set; }
        public BitmapOperationException(string message, Exception ex, string operationName) : base(message, ex)
        {
            this.OperationName = operationName;
        }

        public BitmapOperationException(string nameOfUnsupportedOperation)
        {
            this.OperationName = nameOfUnsupportedOperation;
            OperationIsNotSupported = true;
        }
    }
}
