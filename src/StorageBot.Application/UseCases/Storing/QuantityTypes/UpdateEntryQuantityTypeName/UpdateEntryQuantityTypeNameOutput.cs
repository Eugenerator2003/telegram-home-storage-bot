using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.UpdateEntryQuantityTypeName
{
    public class UpdateEntryQuantityTypeNameOutput : Output
    {
        public UpdateEntryQuantityTypeNameOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
