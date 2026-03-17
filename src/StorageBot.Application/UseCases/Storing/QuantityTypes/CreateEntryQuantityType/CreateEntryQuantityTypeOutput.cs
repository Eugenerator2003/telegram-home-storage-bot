using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.AddEntryQuantityType
{
    public class CreateEntryQuantityTypeOutput : Output
    {
        public CreateEntryQuantityTypeOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
