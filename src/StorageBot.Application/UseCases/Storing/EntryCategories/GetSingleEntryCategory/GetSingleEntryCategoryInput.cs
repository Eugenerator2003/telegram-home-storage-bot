using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.GetSingleEntryCategory
{
    public class GetSingleEntryCategoryInput : StorageInput
    {
        public int EntryCategoryId { get; }

        public GetSingleEntryCategoryInput(int userId, int storageOwnershipId, int entryCategoryId) : base(userId, storageOwnershipId)
        {
            EntryCategoryId = entryCategoryId;
        }
    }
}
