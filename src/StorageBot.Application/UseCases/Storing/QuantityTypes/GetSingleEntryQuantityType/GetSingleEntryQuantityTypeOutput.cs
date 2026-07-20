using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.GetSingleEntryQuantityType
{
    public class GetSingleEntryQuantityTypeOutput : Output
    {
        public EntryQuantityTypeDTO QuantityTypeDTO { get; }

        public GetSingleEntryQuantityTypeOutput(EntryQuantityTypeDTO quantityType, OutputResult result, string message = "")
            : base(result, message)
        {
            QuantityTypeDTO = quantityType;
        }

        public GetSingleEntryQuantityTypeOutput(OutputResult result, string message = "")
            : base(result, message)
        {
            QuantityTypeDTO = new EntryQuantityTypeDTO(-1, false, string.Empty);
        }
    }
}
