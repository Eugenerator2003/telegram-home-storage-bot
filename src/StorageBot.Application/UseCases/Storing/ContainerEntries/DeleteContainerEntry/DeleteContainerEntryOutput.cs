using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries.DeleteContainerEntry
{
    public class DeleteContainerEntryOutput : Output
    {
        public DeleteContainerEntryOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
