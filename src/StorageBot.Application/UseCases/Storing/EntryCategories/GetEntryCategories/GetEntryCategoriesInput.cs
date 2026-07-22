using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.GetEntryCategories
{
    public class GetEntryCategoriesInput : StorageInput
    {
        public int Page { get; }

        public int? MainCategoryId { get; }

        public GetEntryCategoriesInput(int userId, int storageOwnershipId, int? mainCategoryId, int page) 
            : base(userId, storageOwnershipId)
        {
            MainCategoryId = mainCategoryId;
            Page = page;
        }
    }
}
