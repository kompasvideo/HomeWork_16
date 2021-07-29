using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork_16_WPF.Exceptions
{
    public class NoSelectClientException : Exception
    {
        private string messageDetails = string.Empty;
        public NoSelectClientException()
        {
        }

        public NoSelectClientException(string message) : base(message)
        {
            messageDetails = message;
        }
        public override string Message => messageDetails;
    }
}
