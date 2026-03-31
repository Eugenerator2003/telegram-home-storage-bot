using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.DeleteContainer
{
    public class DeleteContainerOutput : Output
    {
        public DeleteContainerOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
