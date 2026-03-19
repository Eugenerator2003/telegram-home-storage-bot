using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.CreateContainer
{
    public class CreateContainerOutput : Output
    {
        public CreateContainerOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
