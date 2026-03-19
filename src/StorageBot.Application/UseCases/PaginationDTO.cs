using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases
{
    public class PaginationDTO
    {
        public int Page { get; }

        public int PageSize { get; }

        public int Count { get; }

        public PaginationDTO(int page, int pageSize, int count)
        {
            Page = page;
            PageSize = pageSize;
            Count = count;
        }
    }
}
