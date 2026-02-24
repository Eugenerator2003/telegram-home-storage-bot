using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.DisallowStorageAccess
{
    public class DisallowStorageAccessOutput : Output
    {
        public DisallowStorageAccessOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
