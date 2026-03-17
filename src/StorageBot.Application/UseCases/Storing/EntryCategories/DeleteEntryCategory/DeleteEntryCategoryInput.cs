using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.DeleteEntryCategoryUseCase
{
    public class DeleteEntryCategoryInput : StorageInput
    {
        public int EntryCategoryId { get; }

        public DeleteEntryCategoryInput(int userId, int storageOwnershipId, int entryCategoryId) 
            : base(userId, storageOwnershipId)
        {
            EntryCategoryId = entryCategoryId;
        }
    }
}
