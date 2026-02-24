using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases
{
    public abstract class Output
    {
        public enum OutputResult
        {
            Success,
            Fail,
            FatalFail
        }

        public string Message { get; }

        public OutputResult Result { get; }

        protected Output(OutputResult result, string message = "")
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(message);
            Message = message;
            Result = result;
        }
    }
}
