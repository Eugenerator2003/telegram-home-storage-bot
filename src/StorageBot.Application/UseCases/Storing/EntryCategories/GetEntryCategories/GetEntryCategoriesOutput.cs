using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.GetEntryCategories
{
    public class GetEntryCategoriesOutput : Output
    {
        public EntryCategoryDTO? MainCategory { get; }

        public EntryCategoryDTO[] SubCategories { get; }

        public PaginationDTO Pagination { get; }

        public GetEntryCategoriesOutput(
            OutputResult result, 
            string message, 
            EntryCategoryDTO? mainCategory, 
            EntryCategoryDTO[] subCategories, 
            PaginationDTO pagination)
            : base(result, message)
        {
            MainCategory = mainCategory;
            SubCategories = subCategories;
            Pagination = pagination;
        }

        public GetEntryCategoriesOutput(OutputResult result, string message)
            : base(result, message)
        {
            SubCategories = Array.Empty<EntryCategoryDTO>();
            Pagination = new PaginationDTO(0, 0, 0);
        }
    }
}
