using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.UpdateEntryCategoryNameUseCase
{
    public class UpdateEntryCategoryNameInput : StorageInput
    {
        public int EntryCategoryId { get; }

        public string CategoryName { get; }

        public UpdateEntryCategoryNameInput(int userId, int storageOwnershipId, int entryCategoryId, string categoryName) 
            : base(userId, storageOwnershipId)
        {
            EntryCategoryId = entryCategoryId;
            CategoryName = categoryName;
        }
    }
}
