using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries.UpdateContainerEntryQuantity
{
    public class UpdateContainerEntryQuantityOutput : Output
    {
        public UpdateContainerEntryQuantityOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
