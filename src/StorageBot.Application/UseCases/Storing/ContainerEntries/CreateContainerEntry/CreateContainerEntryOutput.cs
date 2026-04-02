using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries.CreateContainerEntry
{
    public class CreateContainerEntryOutput : Output
    {
        public CreateContainerEntryOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
