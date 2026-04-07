using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.GetEntryQuantityTypes
{
    public class GetEntryQuantityTypesOutput : Output
    {
        public PaginationDTO Pagination { get; }

        public IReadOnlyCollection<EntryQuantityTypeDTO> EntryQuantityTypes { get; }

        public GetEntryQuantityTypesOutput(
            IEnumerable<EntryQuantityTypeDTO> entryQuantityTypes,
            PaginationDTO pagination,
            OutputResult result, 
            string message = "") 
            : base(result, message)
        {
            EntryQuantityTypes = entryQuantityTypes.ToArray();
            Pagination = pagination;
        }

        public GetEntryQuantityTypesOutput(
            OutputResult result,
            string message = "")
            : base(result, message)
        {
            EntryQuantityTypes = Array.Empty<EntryQuantityTypeDTO>();
            Pagination = new PaginationDTO(0, 0, 0);
        }
    }
}
