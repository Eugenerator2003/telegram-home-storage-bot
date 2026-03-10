using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.RelinquishStorageAccess
{
    public class RelinquishStorageAccessOutput : Output
    {
        public RelinquishStorageAccessOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
