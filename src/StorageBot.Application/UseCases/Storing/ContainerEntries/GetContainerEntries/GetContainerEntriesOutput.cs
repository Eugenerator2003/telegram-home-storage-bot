using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries.GetContainerEntries
{
    public class GetContainerEntriesOutput : Output
    {
        public ContainerEntryDTO[] ContainerEntries { get; }

        public PaginationDTO Pagination { get; }

        public GetContainerEntriesOutput(OutputResult result, string message, IEnumerable<ContainerEntryDTO> entries, PaginationDTO pagination)
            : base(result, message)
        {
            ContainerEntries = entries.ToArray();
            Pagination = pagination;
        }

        public GetContainerEntriesOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
            ContainerEntries = Array.Empty<ContainerEntryDTO>();
            Pagination = new PaginationDTO(0, 0, 0);
        }
    }
}
