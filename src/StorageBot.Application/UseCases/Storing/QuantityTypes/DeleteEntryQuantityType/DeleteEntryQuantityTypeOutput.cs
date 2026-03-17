using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.DeleteEntryQuantityType
{
    public class DeleteEntryQuantityTypeOutput : Output
    {
        public DeleteEntryQuantityTypeOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
