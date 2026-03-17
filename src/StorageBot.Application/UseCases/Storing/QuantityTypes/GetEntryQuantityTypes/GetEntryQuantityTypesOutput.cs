using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.GetEntryQuantityTypes
{
    public class GetEntryQuantityTypesOutput : Output
    {
        public IReadOnlyCollection<EntryQuantityTypeDTO> EntryQuantityTypes { get; }

        public GetEntryQuantityTypesOutput(
            IEnumerable<EntryQuantityTypeDTO> entryQuantityTypes, 
            OutputResult result, 
            string message = "") 
            : base(result, message)
        {
            EntryQuantityTypes = entryQuantityTypes.ToArray();
        }

        public GetEntryQuantityTypesOutput(
            OutputResult result,
            string message = "")
            : base(result, message)
        {
            EntryQuantityTypes = Array.Empty<EntryQuantityTypeDTO>();
        }
    }
}
