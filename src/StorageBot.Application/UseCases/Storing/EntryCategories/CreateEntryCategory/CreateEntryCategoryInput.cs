using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.CreateEntryCategory
{
    public class CreateEntryCategoryInput : StorageInput
    {
        public int? MainCategoryId { get; }

        public string CategoryName { get; }

        public CreateEntryCategoryInput(int userId, int storageOwnershipId, int? mainCategoryId, string categoryName) 
            : base(userId, storageOwnershipId)
        {
            MainCategoryId = mainCategoryId;
            CategoryName = categoryName;
        }
    }
}
