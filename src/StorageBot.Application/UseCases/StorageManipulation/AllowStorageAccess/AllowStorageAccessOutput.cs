using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.AllowStorageAccess
{
    public class AllowStorageAccessOutput : Output
    {
        public AllowStorageAccessOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
